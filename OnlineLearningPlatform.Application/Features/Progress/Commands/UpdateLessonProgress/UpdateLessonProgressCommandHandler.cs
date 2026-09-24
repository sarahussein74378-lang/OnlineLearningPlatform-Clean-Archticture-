using Microsoft.EntityFrameworkCore;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Progress;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
using OnlineLearningPlatform.Domain.Enums;

namespace OnlineLearningPlatform.Application.Features.Progress.Commands.UpdateLessonProgress;

public class UpdateLessonProgressCommandHandler : IRequestHandler<UpdateLessonProgressCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLessonProgressCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdateLessonProgressCommand request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);
        if (student == null) return false;

        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course).ThenInclude(c => c.Lessons)
            .FirstOrDefaultAsync(e => e.CourseId == _unitOfWork.Lessons.Query()
                .Where(l => l.Id == request.Dto.LessonId)
                .Select(l => l.CourseId)
                .FirstOrDefault()
                && e.StudentProfileId == student.Id, cancellationToken);

        if (enrollment == null) return false;

        var progress = await _unitOfWork.LessonProgresses.FindFirstAsync(
            lp => lp.EnrollmentId == enrollment.Id && lp.LessonId == request.Dto.LessonId);

        if (progress == null)
        {
            progress = new LessonProgress
            {
                EnrollmentId = enrollment.Id,
                LessonId = request.Dto.LessonId,
                IsCompleted = request.Dto.IsCompleted,
                WatchedSeconds = request.Dto.WatchedSeconds,
                CompletedAt = request.Dto.IsCompleted ? DateTime.UtcNow : null
            };
            await _unitOfWork.LessonProgresses.AddAsync(progress);
        }
        else
        {
            progress.IsCompleted = request.Dto.IsCompleted;
            progress.WatchedSeconds = request.Dto.WatchedSeconds;
            if (request.Dto.IsCompleted && progress.CompletedAt == null)
                progress.CompletedAt = DateTime.UtcNow;
            _unitOfWork.LessonProgresses.Update(progress);
        }

        await _unitOfWork.SaveChangesAsync();
        await CheckAndCompleteCourseAsync(enrollment);
        return true;
    }

    private async Task CheckAndCompleteCourseAsync(Enrollment enrollment)
    {
        var totalLessons = await _unitOfWork.Lessons.Query()
            .CountAsync(l => l.CourseId == enrollment.CourseId);
        var completedLessons = await _unitOfWork.LessonProgresses.Query()
            .CountAsync(lp => lp.EnrollmentId == enrollment.Id && lp.IsCompleted);

        if (totalLessons > 0 && completedLessons >= totalLessons && enrollment.Status == EnrollmentStatus.Active)
        {
            enrollment.Status = EnrollmentStatus.Completed;
            enrollment.CompletedAt = DateTime.UtcNow;
            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
