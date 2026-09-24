using MediatR;
using OnlineLearningPlatform.Application.DTOs.Lesson;

namespace OnlineLearningPlatform.Application.Features.Lessons.Commands.UpdateLesson;

public record UpdateLessonCommand(int Id, CreateLessonDto Dto) : IRequest<LessonDto?>;
