using OnlineLearningPlatform.Application.DTOs.Quiz;
//using OnlineLearningPlatform.BLL.DTOs.Quiz;

namespace OnlineLearningPlatform.BLL.Services.Interfaces;

public interface IQuizService
{
    Task<QuizDto?> GetByIdAsync(int id);
    Task<IEnumerable<QuizDto>> GetByCourseAsync(int courseId);
    Task<QuizDto> CreateAsync(CreateQuizDto dto);
    Task<bool> DeleteAsync(int id);
    Task<QuizResultDto> SubmitAsync(SubmitQuizDto dto, string studentUserId);
}
