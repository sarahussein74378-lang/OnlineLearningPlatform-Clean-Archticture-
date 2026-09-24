using MediatR;
using OnlineLearningPlatform.Application.DTOs.Profile;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Profiles.Queries.GetStudentProfileByUser;

public class GetStudentProfileByUserQueryHandler : IRequestHandler<GetStudentProfileByUserQuery, StudentProfileDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentProfileByUserQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<StudentProfileDto?> Handle(GetStudentProfileByUserQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.UserId);
        if (student == null) return null;

        return new StudentProfileDto
        {
            Id = student.Id,
            UserId = student.UserId,
            EnrolledSince = student.EnrolledSince
        };
    }
}
