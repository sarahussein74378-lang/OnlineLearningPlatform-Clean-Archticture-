using MediatR;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.DeleteCourse;

public record DeleteCourseCommand(int Id, string InstructorUserId) : IRequest<bool>;
