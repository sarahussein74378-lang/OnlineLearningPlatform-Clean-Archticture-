using MediatR;

namespace OnlineLearningPlatform.Application.Features.Enrollments.Commands.Drop;

public record DropEnrollmentCommand(int EnrollmentId, string StudentUserId) : IRequest<bool>;
