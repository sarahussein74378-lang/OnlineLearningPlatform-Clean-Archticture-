using MediatR;
using OnlineLearningPlatform.Application.DTOs.Enrollment;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Application.Features.Enrollments.Queries.GetStudentEnrollments;

public record GetStudentEnrollmentsQuery(string StudentUserId) : IRequest<IEnumerable<EnrollmentDto>>;
