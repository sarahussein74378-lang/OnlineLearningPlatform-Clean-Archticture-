using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Certificate;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Certificates.Queries.GetStudentCertificates;

public class GetStudentCertificatesQueryHandler : IRequestHandler<GetStudentCertificatesQuery, IEnumerable<CertificateDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentCertificatesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CertificateDto>> Handle(GetStudentCertificatesQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);
        if (student == null) return Array.Empty<CertificateDto>();

        var certs = await _unitOfWork.Certificates.Query()
            .Include(c => c.Student).ThenInclude(s => s.User)
            .Include(c => c.Enrollment).ThenInclude(e => e.Course)
            .ThenInclude(c => c.Instructor).ThenInclude(i => i.User)
            .Where(c => c.StudentProfileId == student.Id)
            .ToListAsync(cancellationToken);

        return certs.Select(c => new CertificateDto
        {
            Id = c.Id,
            CertificateNumber = c.CertificateNumber,
            IssuedAt = c.IssuedAt,
            StudentName = c.Student != null ? $"{c.Student.User.FirstName} {c.Student.User.LastName}" : string.Empty,
            CourseTitle = c.Enrollment?.Course?.Title ?? string.Empty,
            InstructorName = c.Enrollment?.Course?.Instructor?.User != null ? $"{c.Enrollment.Course.Instructor.User.FirstName} {c.Enrollment.Course.Instructor.User.LastName}" : string.Empty
        });
    }
}
