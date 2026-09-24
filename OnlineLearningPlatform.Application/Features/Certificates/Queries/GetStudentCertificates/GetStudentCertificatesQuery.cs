using MediatR;
using OnlineLearningPlatform.Application.DTOs.Certificate;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Application.Features.Certificates.Queries.GetStudentCertificates;

public record GetStudentCertificatesQuery(string StudentUserId) : IRequest<IEnumerable<CertificateDto>>;
