using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface IQuizRepository : IGenericRepository<Quiz>
{
    Task<Quiz?> GetQuizWithQuestionsAndAnswersAsync(int id);
    Task<IEnumerable<Quiz>> GetQuizzesByCourseIdAsync(int courseId);
}
