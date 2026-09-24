using MediatR;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Course;

namespace OnlineLearningPlatform.Application.Features.Courses.Queries.GetAllCourses;

public record GetAllCoursesQuery(int Page = 1, int PageSize = 10, string? Category = null, string? Level = null)
    : IRequest<PaginatedResult<CourseDto>>;
