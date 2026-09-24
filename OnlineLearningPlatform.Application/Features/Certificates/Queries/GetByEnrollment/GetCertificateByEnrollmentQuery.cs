using MediatR;
using OnlineLearningPlatform.Application.DTOs.Certificate;

namespace OnlineLearningPlatform.Application.Features.Certificates.Queries.GetByEnrollment;

public record GetCertificateByEnrollmentQuery(int EnrollmentId) : IRequest<CertificateDto?>;
