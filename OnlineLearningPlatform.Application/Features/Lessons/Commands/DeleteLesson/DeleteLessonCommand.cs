using MediatR;

namespace OnlineLearningPlatform.Application.Features.Lessons.Commands.DeleteLesson;

public record DeleteLessonCommand(int Id) : IRequest<bool>;
