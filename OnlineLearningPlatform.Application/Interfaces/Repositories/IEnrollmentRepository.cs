using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<Enrollment?> GetEnrollmentWithDetailsAsync(int enrollmentId);
    Task<IEnumerable<Enrollment>> GetStudentEnrollmentsAsync(string studentUserId);
}
