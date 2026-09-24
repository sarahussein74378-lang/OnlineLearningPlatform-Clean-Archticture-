using OnlineLearningPlatform.BLL.DTOs.Enrollment;

namespace OnlineLearningPlatform.BLL.Services.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentDto?> EnrollAsync(int courseId, string studentUserId);
    Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(string studentUserId);
    Task<bool> DropCourseAsync(int enrollmentId, string studentUserId);
}
