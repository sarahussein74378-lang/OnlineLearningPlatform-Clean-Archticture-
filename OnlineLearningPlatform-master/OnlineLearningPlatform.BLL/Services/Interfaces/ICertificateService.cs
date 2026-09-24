using OnlineLearningPlatform.BLL.DTOs.Certificate;

namespace OnlineLearningPlatform.BLL.Services.Interfaces;

public interface ICertificateService
{
    Task<CertificateDto?> GetByEnrollmentAsync(int enrollmentId);
    Task<IEnumerable<CertificateDto>> GetStudentCertificatesAsync(string studentUserId);
    Task<CertificateDto?> IssueCertificateAsync(int enrollmentId);
    Task<byte[]?> GeneratePdfAsync(int certificateId);
}
