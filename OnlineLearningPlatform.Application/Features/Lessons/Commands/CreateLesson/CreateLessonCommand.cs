using MediatR;
using OnlineLearningPlatform.Application.DTOs.Lesson;

namespace OnlineLearningPlatform.Application.Features.Lessons.Commands.CreateLesson;

public record CreateLessonCommand(CreateLessonDto Dto) : IRequest<LessonDto>;
