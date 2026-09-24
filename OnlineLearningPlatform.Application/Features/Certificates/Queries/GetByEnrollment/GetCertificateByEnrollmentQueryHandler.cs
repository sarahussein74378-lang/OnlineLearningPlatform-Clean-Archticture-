using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Certificate;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Features.Certificates.Queries.GetByEnrollment;

public class GetCertificateByEnrollmentQueryHandler : IRequestHandler<GetCertificateByEnrollmentQuery, CertificateDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCertificateByEnrollmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CertificateDto?> Handle(GetCertificateByEnrollmentQuery request, CancellationToken cancellationToken)
    {
        var cert = await _unitOfWork.Certificates.Query()
            .Include(c => c.Student).ThenInclude(s => s.User)
            .Include(c => c.Enrollment).ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .FirstOrDefaultAsync(c => c.EnrollmentId == request.EnrollmentId, cancellationToken);

        if (cert == null) return null;

        // Map manually similar to existing service mapping
        return new CertificateDto
        {
            Id = cert.Id,
            CertificateNumber = cert.CertificateNumber,
            IssuedAt = cert.IssuedAt,
            StudentName = cert.Student != null ? $"{cert.Student.User.FirstName} {cert.Student.User.LastName}" : string.Empty,
            CourseTitle = cert.Enrollment?.Course?.Title ?? string.Empty,
            InstructorName = cert.Enrollment?.Course?.Instructor?.User != null ? $"{cert.Enrollment.Course.Instructor.User.FirstName} {cert.Enrollment.Course.Instructor.User.LastName}" : string.Empty
        };
    }
}
