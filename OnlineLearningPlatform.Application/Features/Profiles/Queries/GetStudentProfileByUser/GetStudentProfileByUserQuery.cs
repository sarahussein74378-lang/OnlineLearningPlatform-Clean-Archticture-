using MediatR;
using OnlineLearningPlatform.Application.DTOs.Profile;

namespace OnlineLearningPlatform.Application.Features.Profiles.Queries.GetStudentProfileByUser;

public record GetStudentProfileByUserQuery(string UserId) : IRequest<StudentProfileDto?>;
