using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface IInstructorProfileRepository : IGenericRepository<InstructorProfile>
{
    Task<InstructorProfile?> GetByUserIdAsync(string userId);
}
