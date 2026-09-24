using MediatR;
using OnlineLearningPlatform.Application.DTOs.Profile;

namespace OnlineLearningPlatform.Application.Features.Profiles.Queries.GetInstructorProfileByUser;

public record GetInstructorProfileByUserQuery(string UserId) : IRequest<InstructorProfileDto?>;
