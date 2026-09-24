using MediatR;
using OnlineLearningPlatform.Application.DTOs.Enrollment;

namespace OnlineLearningPlatform.Application.Features.Enrollments.Commands.Enroll;

public record EnrollCommand(int CourseId, string StudentUserId) : IRequest<EnrollmentDto?>;
