using MediatR;
using OnlineLearningPlatform.Application.DTOs.Course;

namespace OnlineLearningPlatform.Application.Features.Courses.Queries.GetCourseById;

public record GetCourseByIdQuery(int Id) : IRequest<CourseDto?>;
