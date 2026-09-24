using MediatR;
using OnlineLearningPlatform.Application.DTOs.Profile;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Profiles.Queries.GetInstructorProfileByUser;

public class GetInstructorProfileByUserQueryHandler : IRequestHandler<GetInstructorProfileByUserQuery, InstructorProfileDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstructorProfileByUserQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<InstructorProfileDto?> Handle(GetInstructorProfileByUserQuery request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.InstructorProfiles.FindFirstAsync(i => i.UserId == request.UserId);
        if (instructor == null) return null;

        return new InstructorProfileDto
        {
            Id = instructor.Id,
            UserId = instructor.UserId,
            Bio = instructor.Bio,
            Expertise = instructor.Expertise,
            Rating = instructor.Rating,
            TotalStudents = instructor.TotalStudents
        };
    }
}
