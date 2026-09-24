using MediatR;
using OnlineLearningPlatform.Application.DTOs.Progress;

namespace OnlineLearningPlatform.Application.Features.Progress.Commands.UpdateLessonProgress;

public record UpdateLessonProgressCommand(UpdateLessonProgressDto Dto, string StudentUserId) : IRequest<bool>;
