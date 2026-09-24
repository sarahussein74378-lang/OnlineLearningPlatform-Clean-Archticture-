using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface ILessonRepository : IGenericRepository<Lesson>
{
    Task<IEnumerable<Lesson>> GetLessonsByCourseIdAsync(int courseId);
}
