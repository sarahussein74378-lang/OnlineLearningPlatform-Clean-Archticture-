using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<(IEnumerable<Course> Items, int TotalCount)> GetPublishedCoursesPagedAsync(int page, int pageSize, string? category, string? level);
    Task<Course?> GetCourseWithDetailsAsync(int id);
    Task<IEnumerable<Course>> GetCoursesByInstructorUserIdAsync(string instructorUserId);
}
