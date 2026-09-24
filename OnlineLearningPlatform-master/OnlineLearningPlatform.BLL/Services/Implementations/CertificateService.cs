using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineLearningPlatform.BLL.DTOs.Certificate;
using OnlineLearningPlatform.BLL.Services.Interfaces;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.UnitOfWork;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class CertificateService : ICertificateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public CertificateService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<CertificateDto?> GetByEnrollmentAsync(int enrollmentId)
    {
        var cert = await _unitOfWork.Certificates.Query()
            .Include(c => c.Student).ThenInclude(s => s.User)
            .Include(c => c.Enrollment).ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .FirstOrDefaultAsync(c => c.EnrollmentId == enrollmentId);

       

        return cert == null ? null : MapToDto(cert);
    }

    public async Task<IEnumerable<CertificateDto>> GetStudentCertificatesAsync(string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);
        if (student == null) return [];

        var certs = await _unitOfWork.Certificates.Query()
            .Include(c => c.Student).ThenInclude(s => s.User)
            .Include(c => c.Enrollment).ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .Where(c => c.StudentProfileId == student.Id)
            .ToListAsync();

        return certs.Select(c => MapToDto(c));
    }

    public async Task<CertificateDto?> IssueCertificateAsync(int enrollmentId)
    {
        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course).ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .Include(e => e.Student).ThenInclude(s => s.User)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null || enrollment.Status != EnrollmentStatus.Completed)
            return null;

        var existing = await _unitOfWork.Certificates.ExistsAsync(c => c.EnrollmentId == enrollmentId);
        if (existing) return await GetByEnrollmentAsync(enrollmentId);

        var certNumber = $"CERT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        var pdfDir = _configuration["Certificates:StoragePath"] ?? "wwwroot/certificates";
        Directory.CreateDirectory(pdfDir);
        var pdfPath = Path.Combine(pdfDir, $"{certNumber}.pdf");

        var certificate = new Certificate
        {
            StudentProfileId = enrollment.StudentProfileId,
            EnrollmentId = enrollmentId,
            CertificateNumber = certNumber,
            PdfPath = pdfPath
        };

        await _unitOfWork.Certificates.AddAsync(certificate);
        await _unitOfWork.SaveChangesAsync();

        var dto = MapToDto(certificate, enrollment);
        var pdfBytes = GeneratePdfDocument(dto);
        await File.WriteAllBytesAsync(pdfPath, pdfBytes);

        return dto;
    }

    public async Task<byte[]?> GeneratePdfAsync(int certificateId)
    {
        var cert = await _unitOfWork.Certificates.Query()
            .Include(c => c.Student).ThenInclude(s => s.User)
            .Include(c => c.Enrollment).ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .FirstOrDefaultAsync(c => c.Id == certificateId);

        if (cert == null) return null;

        if (File.Exists(cert.PdfPath))
            return await File.ReadAllBytesAsync(cert.PdfPath);

        return GeneratePdfDocument(MapToDto(cert));
    }

    private static byte[] GeneratePdfDocument(CertificateDto dto)
    {
        return Document.Create(container =>
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

    private static CertificateDto MapToDto(Certificate c, Enrollment? enrollment = null)
    {
        var enr = enrollment ?? c.Enrollment;
        return new CertificateDto
        {
            Id = c.Id,
            CertificateNumber = c.CertificateNumber,
            IssuedAt = c.IssuedAt,
            StudentName = c.Student != null
                ? $"{c.Student.User.FirstName} {c.Student.User.LastName}"
                : string.Empty,
            CourseTitle = enr?.Course?.Title ?? string.Empty,
            InstructorName = enr?.Course?.Instructor?.User != null
                ? $"{enr.Course.Instructor.User.FirstName} {enr.Course.Instructor.User.LastName}"
                : string.Empty
        };
    }
}
