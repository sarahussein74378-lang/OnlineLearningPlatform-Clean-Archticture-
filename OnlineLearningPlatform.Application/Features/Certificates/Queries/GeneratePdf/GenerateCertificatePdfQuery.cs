using MediatR;

namespace OnlineLearningPlatform.Application.Features.Certificates.Queries.GeneratePdf;

public record GenerateCertificatePdfQuery(int CertificateId) : IRequest<byte[]?>;
