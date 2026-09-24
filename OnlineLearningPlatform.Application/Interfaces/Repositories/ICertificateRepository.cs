using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface ICertificateRepository : IGenericRepository<Certificate>
{
    Task<Certificate?> GetByEnrollmentIdAsync(int enrollmentId);
    Task<IEnumerable<Certificate>> GetByStudentProfileIdAsync(int studentProfileId);
    Task<Certificate?> GetWithDetailsByIdAsync(int certificateId);
}
