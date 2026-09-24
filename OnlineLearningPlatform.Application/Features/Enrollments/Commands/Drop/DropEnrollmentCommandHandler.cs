using Microsoft.EntityFrameworkCore;
using MediatR;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Enrollments.Commands.Drop;

public class DropEnrollmentCommandHandler : IRequestHandler<DropEnrollmentCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DropEnrollmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DropEnrollmentCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);
        var enrollment = await _unitOfWork.Enrollments.FindFirstAsync(
            e => e.Id == request.EnrollmentId && e.StudentProfileId == student!.Id);

        if (enrollment == null) return false;

        enrollment.Status = OnlineLearningPlatform.Domain.Enums.EnrollmentStatus.Dropped;
        _unitOfWork.Enrollments.Update(enrollment);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
