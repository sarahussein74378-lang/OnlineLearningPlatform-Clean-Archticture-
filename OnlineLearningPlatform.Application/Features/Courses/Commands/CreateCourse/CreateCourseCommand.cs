using MediatR;
using OnlineLearningPlatform.Application.DTOs.Course;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.CreateCourse;

public record CreateCourseCommand(CreateCourseDto Dto, string InstructorUserId) : IRequest<CourseDto>;
