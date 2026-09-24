using MediatR;
using OnlineLearningPlatform.Application.DTOs.Progress;

namespace OnlineLearningPlatform.Application.Features.Progress.Queries.GetProgress;

public record GetProgressQuery(int EnrollmentId, string StudentUserId) : IRequest<ProgressDto?>;
