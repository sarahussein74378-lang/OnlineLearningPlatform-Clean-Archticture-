using Microsoft.EntityFrameworkCore;
using MediatR;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Application.DTOs.Certificate;

namespace OnlineLearningPlatform.Application.Features.Certificates.Queries.GeneratePdf;

public class GenerateCertificatePdfQueryHandler : IRequestHandler<GenerateCertificatePdfQuery, byte[]?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GenerateCertificatePdfQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]?> Handle(GenerateCertificatePdfQuery request, CancellationToken cancellationToken)
    {
        var cert = await _unitOfWork.Certificates.Query()
            .Include(c => c.Student).ThenInclude(s => s.User)
            .Include(c => c.Enrollment).ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .FirstOrDefaultAsync(c => c.Id == request.CertificateId, cancellationToken);

        if (cert == null) return null;

        if (File.Exists(cert.PdfPath))
            return await File.ReadAllBytesAsync(cert.PdfPath, cancellationToken);

        // Generate pdf bytes similar to earlier GeneratePdfDocument
        var dto = new CertificateDto
        {
            Id = cert.Id,
            CertificateNumber = cert.CertificateNumber,
            IssuedAt = cert.IssuedAt,
            StudentName = cert.Student != null ? $"{cert.Student.User.FirstName} {cert.Student.User.LastName}" : string.Empty,
            CourseTitle = cert.Enrollment?.Course?.Title ?? string.Empty,
            InstructorName = cert.Enrollment?.Course?.Instructor?.User != null ? $"{cert.Enrollment.Course.Instructor.User.FirstName} {cert.Enrollment.Course.Instructor.User.LastName}" : string.Empty
        };

        return OnlineLearningPlatform.Application.Features.Certificates.Commands.IssueCertificate.IssueCertificateCommandHandler.GeneratePdfDocument(dto);
    }
}
