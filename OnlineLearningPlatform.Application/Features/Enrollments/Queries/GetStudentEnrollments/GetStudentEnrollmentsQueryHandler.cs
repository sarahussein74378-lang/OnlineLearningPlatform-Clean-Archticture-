using Microsoft.EntityFrameworkCore;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Enrollment;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Enrollments.Queries.GetStudentEnrollments;

public class GetStudentEnrollmentsQueryHandler : IRequestHandler<GetStudentEnrollmentsQuery, IEnumerable<EnrollmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentEnrollmentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EnrollmentDto>> Handle(GetStudentEnrollmentsQuery request, CancellationToken cancellationToken)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == request.StudentUserId);
        if (student == null) return Array.Empty<EnrollmentDto>();

        var enrollments = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course)
            .Include(e => e.LessonProgresses).ThenInclude(lp => lp.Lesson)
            .Where(e => e.StudentProfileId == student.Id)
            .ToListAsync(cancellationToken);

        return enrollments.Select(e => new EnrollmentDto
        {
            Id = e.Id,
            CourseId = e.CourseId,
            CourseTitle = e.Course?.Title ?? string.Empty,
            EnrolledAt = e.EnrolledAt,
            CompletedAt = e.CompletedAt,
            Status = e.Status.ToString(),
            ProgressPercentage = e.LessonProgresses.Count == 0 ? 0 : Math.Round((double)e.LessonProgresses.Count(lp => lp.IsCompleted) / e.LessonProgresses.Count * 100, 1)
        });
    }
}
