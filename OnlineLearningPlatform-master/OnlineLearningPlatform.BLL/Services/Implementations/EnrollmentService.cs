using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.BLL.DTOs.Enrollment;
using OnlineLearningPlatform.BLL.Services.Interfaces;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.UnitOfWork;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EnrollmentDto?> EnrollAsync(int courseId, string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);
        if (student == null) return null;

        var alreadyEnrolled = await _unitOfWork.Enrollments.ExistsAsync(
            e => e.StudentProfileId == student.Id && e.CourseId == courseId);
        if (alreadyEnrolled) return null;

        var enrollment = new Enrollment
        {
            StudentProfileId = student.Id,
            CourseId = courseId,
            Status = EnrollmentStatus.Active
        };

        await _unitOfWork.Enrollments.AddAsync(enrollment);
        await _unitOfWork.SaveChangesAsync();

        return await GetEnrollmentDtoAsync(enrollment.Id);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);
        if (student == null) return [];

        var enrollments = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course)
            .Include(e => e.LessonProgresses)
            .ThenInclude(lp => lp.Lesson)
            .Where(e => e.StudentProfileId == student.Id)
            .ToListAsync();

        return enrollments.Select(e => MapToDto(e));
    }

    public async Task<bool> DropCourseAsync(int enrollmentId, string studentUserId)
    {
        var student = await _unitOfWork.StudentProfiles.FindFirstAsync(s => s.UserId == studentUserId);
        var enrollment = await _unitOfWork.Enrollments.FindFirstAsync(
            e => e.Id == enrollmentId && e.StudentProfileId == student!.Id);

        if (enrollment == null) return false;

        enrollment.Status = EnrollmentStatus.Dropped;
        _unitOfWork.Enrollments.Update(enrollment);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private async Task<EnrollmentDto?> GetEnrollmentDtoAsync(int enrollmentId)
    {
        var enrollment = await _unitOfWork.Enrollments.Query()
            .Include(e => e.Course)
            .Include(e => e.LessonProgresses)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        return enrollment == null ? null : MapToDto(enrollment);
    }

    private static EnrollmentDto MapToDto(Enrollment e)
    {
        var totalLessons = e.LessonProgresses.Count;
        var completed = e.LessonProgresses.Count(lp => lp.IsCompleted);
        return new EnrollmentDto
        {
            Id = e.Id,
            CourseId = e.CourseId,
            CourseTitle = e.Course?.Title ?? string.Empty,
            EnrolledAt = e.EnrolledAt,
            CompletedAt = e.CompletedAt,
            Status = e.Status.ToString(),
            ProgressPercentage = totalLessons == 0 ? 0 : Math.Round((double)completed / totalLessons * 100, 1)
        };
    }
}
