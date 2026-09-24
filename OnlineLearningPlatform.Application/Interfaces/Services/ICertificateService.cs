using OnlineLearningPlatform.Application.DTOs.Certificate;

namespace OnlineLearningPlatform.Application.Interfaces.Services;

public interface ICertificateService
{
    Task<CertificateDto?> GetByEnrollmentAsync(int enrollmentId);
    Task<IEnumerable<CertificateDto>> GetStudentCertificatesAsync(string studentUserId);
    Task<CertificateDto?> IssueCertificateAsync(int enrollmentId);
    Task<byte[]?> GeneratePdfAsync(int certificateId);
}
