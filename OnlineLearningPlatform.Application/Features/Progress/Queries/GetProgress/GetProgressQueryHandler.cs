using Microsoft.EntityFrameworkCore;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Progress;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Progress.Queries.GetProgress;

public class GetProgressQueryHandler : IRequestHandler<GetProgressQuery, ProgressDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProgressQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ProgressDto?> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);
        if (student == null) return null;

        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course).ThenInclude(c => c.Lessons)
            .Include(e => e.LessonProgresses).ThenInclude(lp => lp.Lesson)
            .FirstOrDefaultAsync(e => e.Id == request.EnrollmentId && e.StudentProfileId == student.Id, cancellationToken);

        if (enrollment == null) return null;

        var totalLessons = enrollment.Course.Lessons.Count;
        var completedLessons = enrollment.LessonProgresses.Count(lp => lp.IsCompleted);

        return new ProgressDto
        {
            EnrollmentId = request.EnrollmentId,
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
}
