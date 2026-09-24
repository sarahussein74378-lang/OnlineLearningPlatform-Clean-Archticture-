using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using OnlineLearningPlatform.Application.DTOs.Certificate;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Domain.Enums;

namespace OnlineLearningPlatform.Application.Features.Certificates.Commands.IssueCertificate;

public class IssueCertificateCommandHandler : IRequestHandler<IssueCertificateCommand, CertificateDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public IssueCertificateCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<CertificateDto?> Handle(IssueCertificateCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course).ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .Include(e => e.Student).ThenInclude(s => s.User)
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId, cancellationToken);

        if (enrollment == null || enrollment.Status != EnrollmentStatus.Completed)
            return null;

        var existing = await _unitOfWork.Certificates.ExistsAsync(c => c.EnrollmentId == request.EnrollmentId);
        if (existing) return await new GetByEnrollmentProxy(_unitOfWork).Get(request.EnrollmentId);

        var certNumber = $"CERT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        var pdfDir = _configuration["Certificates:StoragePath"] ?? "wwwroot/certificates";
        Directory.CreateDirectory(pdfDir);
        var pdfPath = Path.Combine(pdfDir, $"{certNumber}.pdf");

        var certificate = new Certificate
        {
            StudentProfileId = enrollment.StudentProfileId,
            EnrollmentId = request.EnrollmentId,
            CertificateNumber = certNumber,
            PdfPath = pdfPath
        };

        await _unitOfWork.Certificates.AddAsync(certificate);
        await _unitOfWork.SaveChangesAsync();

        var dto = new CertificateDto
        {
            Id = certificate.Id,
            CertificateNumber = certificate.CertificateNumber,
            IssuedAt = certificate.IssuedAt,
            StudentName = enrollment.Student != null ? $"{enrollment.Student.User.FirstName} {enrollment.Student.User.LastName}" : string.Empty,
            CourseTitle = enrollment.Course?.Title ?? string.Empty,
            InstructorName = enrollment.Course?.Instructor?.User != null ? $"{enrollment.Course.Instructor.User.FirstName} {enrollment.Course.Instructor.User.LastName}" : string.Empty
        };

        var pdfBytes = GeneratePdfDocument(dto);
        await File.WriteAllBytesAsync(pdfPath, pdfBytes, cancellationToken);

        return dto;
    }

    public static byte[] GeneratePdfDocument(CertificateDto dto)
    {
        return QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);
                page.Background().Background("#FFFFF8");

                page.Content().Column(col =>
                {
                    col.Item().AlignCenter().Text("Certificate of Completion")
                        .FontSize(36).Bold().FontColor("#1A1A6E");

                    col.Item().Height(20);
                    col.Item().AlignCenter().Text("This is to certify that")
                        .FontSize(16).FontColor("#555555");

                    col.Item().Height(10);
                    col.Item().AlignCenter().Text(dto.StudentName)
                        .FontSize(28).Bold().FontColor("#C0392B");

                    col.Item().Height(10);
                    col.Item().AlignCenter().Text("has successfully completed the course")
                        .FontSize(16).FontColor("#555555");

                    col.Item().Height(10);
                    col.Item().AlignCenter().Text(dto.CourseTitle)
                        .FontSize(22).Bold().FontColor("#1A1A6E");

                    col.Item().Height(20);
                    col.Item().AlignCenter().Text($"Instructor: {dto.InstructorName}")
                        .FontSize(14).FontColor("#555555");

                    col.Item().Height(10);
                    col.Item().AlignCenter().Text($"Issued on: {dto.IssuedAt:MMMM dd, yyyy}")
                        .FontSize(13).FontColor("#777777");

                    col.Item().Height(10);
                    col.Item().AlignCenter().Text($"Certificate No: {dto.CertificateNumber}")
                        .FontSize(11).FontColor("#999999");
                });
            });
        }).GeneratePdf();
    }

    // Small proxy to reuse GetByEnrollment logic (avoids circular DI to query handler)
    private class GetByEnrollmentProxy
    {
        private readonly IUnitOfWork _uow;
        public GetByEnrollmentProxy(IUnitOfWork uow) => _uow = uow;
        public async Task<CertificateDto?> Get(int enrollmentId)
        {
            var cert = await _uow.Certificates.Query()
                .Include(c => c.Student).ThenInclude(s => s.User)
                .Include(c => c.Enrollment).ThenInclude(e => e.Course)
                .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
                .FirstOrDefaultAsync(c => c.EnrollmentId == enrollmentId);

            if (cert == null) return null;

            return new CertificateDto
            {
                Id = cert.Id,
                CertificateNumber = cert.CertificateNumber,
                IssuedAt = cert.IssuedAt,
                StudentName = cert.Student != null ? $"{cert.Student.User.FirstName} {cert.Student.User.LastName}" : string.Empty,
                CourseTitle = cert.Enrollment?.Course?.Title ?? string.Empty,
                InstructorName = cert.Enrollment?.Course?.Instructor?.User != null ? $"{cert.Enrollment.Course.Instructor.User.FirstName} {cert.Enrollment.Course.Instructor.User.LastName}" : string.Empty
            };
        }
    }
}
