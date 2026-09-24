using MediatR;
using OnlineLearningPlatform.Application.DTOs.Certificate;

namespace OnlineLearningPlatform.Application.Features.Certificates.Commands.IssueCertificate;

public record IssueCertificateCommand(int EnrollmentId) : IRequest<CertificateDto?>;
