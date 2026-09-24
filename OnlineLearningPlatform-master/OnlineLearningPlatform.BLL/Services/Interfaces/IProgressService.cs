using OnlineLearningPlatform.BLL.DTOs.Progress;

namespace OnlineLearningPlatform.BLL.Services.Interfaces;

public interface IProgressService
{
    Task<ProgressDto?> GetProgressAsync(int enrollmentId, string studentUserId);
    Task<bool> UpdateLessonProgressAsync(UpdateLessonProgressDto dto, string studentUserId);
}
