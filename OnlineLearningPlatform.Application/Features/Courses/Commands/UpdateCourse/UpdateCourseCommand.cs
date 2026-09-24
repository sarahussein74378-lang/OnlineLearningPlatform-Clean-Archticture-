using MediatR;
using OnlineLearningPlatform.Application.DTOs.Course;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.UpdateCourse;

public record UpdateCourseCommand(int Id, UpdateCourseDto Dto, string InstructorUserId) : IRequest<CourseDto?>;
