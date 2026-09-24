using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.BLL.DTOs.Progress;
using OnlineLearningPlatform.BLL.Services.Interfaces;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.UnitOfWork;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class ProgressService : IProgressService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProgressService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ProgressDto?> GetProgressAsync(int enrollmentId, string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);
        if (student == null) return null;

        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course).ThenInclude(c => c.Lessons)
            .Include(e => e.LessonProgresses).ThenInclude(lp => lp.Lesson)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId && e.StudentProfileId == student.Id);

        if (enrollment == null) return null;

        var totalLessons = enrollment.Course.Lessons.Count;
        var completedLessons = enrollment.LessonProgresses.Count(lp => lp.IsCompleted);

        return new ProgressDto
        {
            EnrollmentId = enrollmentId,
            CourseId = enrollment.CourseId,
            CourseTitle = enrollment.Course.Title,
            TotalLessons = totalLessons,
            CompletedLessons = completedLessons,
            ProgressPercentage = totalLessons == 0 ? 0 : Math.Round((double)completedLessons / totalLessons * 100, 1),
            LessonProgresses = enrollment.LessonProgresses.Select(lp => new LessonProgressDto
            {
                LessonId = lp.LessonId,
                LessonTitle = lp.Lesson.Title,
                IsCompleted = lp.IsCompleted,
                CompletedAt = lp.CompletedAt,
                WatchedSeconds = lp.WatchedSeconds
            })
        };
    }

    public async Task<bool> UpdateLessonProgressAsync(UpdateLessonProgressDto dto, string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);
        if (student == null) return false;

        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course).ThenInclude(c => c.Lessons)
            .FirstOrDefaultAsync(e => e.CourseId == _unitOfWork.Lessons.Query()
                .Where(l => l.Id == dto.LessonId)
                .Select(l => l.CourseId)
                .FirstOrDefault()
                && e.StudentProfileId == student.Id);

        if (enrollment == null) return false;

        var progress = await _unitOfWork.LessonProgresses.FindFirstAsync(
            lp => lp.EnrollmentId == enrollment.Id && lp.LessonId == dto.LessonId);

        if (progress == null)
        {
            progress = new LessonProgress
            {
                EnrollmentId = enrollment.Id,
                LessonId = dto.LessonId,
                IsCompleted = dto.IsCompleted,
                WatchedSeconds = dto.WatchedSeconds,
                CompletedAt = dto.IsCompleted ? DateTime.UtcNow : null
            };
            await _unitOfWork.LessonProgresses.AddAsync(progress);
        }
        else
        {
            progress.IsCompleted = dto.IsCompleted;
            progress.WatchedSeconds = dto.WatchedSeconds;
            if (dto.IsCompleted && progress.CompletedAt == null)
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
