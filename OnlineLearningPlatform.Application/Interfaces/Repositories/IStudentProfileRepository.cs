using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface IStudentProfileRepository : IGenericRepository<StudentProfile>
{
    Task<StudentProfile?> GetByUserIdAsync(string userId);
}
