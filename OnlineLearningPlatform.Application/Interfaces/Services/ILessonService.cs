using OnlineLearningPlatform.Application.DTOs.Lesson;
//
namespace OnlineLearningPlatform.Application.Interfaces.Services;

public interface ILessonService
{
    Task<IEnumerable<LessonDto>> GetByCourseAsync(int courseId);
    Task<LessonDto?> GetByIdAsync(int id);
    Task<LessonDto> CreateAsync(CreateLessonDto dto);
    Task<LessonDto?> UpdateAsync(int id, CreateLessonDto dto);
    Task<bool> DeleteAsync(int id);
}
