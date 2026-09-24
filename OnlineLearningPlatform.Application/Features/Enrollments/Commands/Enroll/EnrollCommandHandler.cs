using Microsoft.EntityFrameworkCore;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Enrollment;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Domain.Enums;

namespace OnlineLearningPlatform.Application.Features.Enrollments.Commands.Enroll;

public class EnrollCommandHandler : IRequestHandler<EnrollCommand, EnrollmentDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public EnrollCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EnrollmentDto?> Handle(EnrollCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);
        if (student == null) return null;

        var alreadyEnrolled = await _unitOfWork.Enrollments.ExistsAsync(
            e => e.StudentProfileId == student.Id && e.CourseId == request.CourseId);
        if (alreadyEnrolled) return null;

        var enrollment = new Enrollment
        {
            StudentProfileId = student.Id,
            CourseId = request.CourseId,
            Status = EnrollmentStatus.Active
        };

        await _unitOfWork.Enrollments.AddAsync(enrollment);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course)
            .Include(e => e.LessonProgresses)
            .FirstOrDefaultAsync(e => e.Id == enrollment.Id, cancellationToken);

        if (created == null) return null;

        var totalLessons = created.LessonProgresses.Count;
        var completed = created.LessonProgresses.Count(lp => lp.IsCompleted);

        return new EnrollmentDto
        {
            Id = created.Id,
            CourseId = created.CourseId,
            CourseTitle = created.Course?.Title ?? string.Empty,
            EnrolledAt = created.EnrolledAt,
            CompletedAt = created.CompletedAt,
            Status = created.Status.ToString(),
            ProgressPercentage = totalLessons == 0 ? 0 : Math.Round((double)completed / totalLessons * 100, 1)
        };
    }
}
