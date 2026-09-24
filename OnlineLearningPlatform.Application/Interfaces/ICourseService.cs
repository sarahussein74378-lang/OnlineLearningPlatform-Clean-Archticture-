using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Course;
//using OnlineLearningPlatform.BLL.Common;
//using OnlineLearningPlatform.BLL.DTOs.Course;

namespace OnlineLearningPlatform.BLL.Services.Interfaces;

public interface ICourseService
{
    Task<PaginatedResult<CourseDto>> GetAllAsync(int page, int pageSize, string? category = null, string? level = null);
    Task<CourseDto?> GetByIdAsync(int id);
    Task<CourseDto> CreateAsync(CreateCourseDto dto, string instructorUserId);
    Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto, string instructorUserId);
    Task<bool> DeleteAsync(int id, string instructorUserId);
    Task<IEnumerable<CourseDto>> GetByInstructorAsync(string instructorUserId);
}
