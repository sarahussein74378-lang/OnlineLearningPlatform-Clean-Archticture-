//using AutoMapper;
using MapsterMapper;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Course;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.UpdateCourse;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CourseDto?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.InstructorProfiles.GetByUserIdAsync(request.InstructorUserId);
        if (instructor == null) return null;

        var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Id == request.Id && c.InstructorProfileId == instructor.Id);
        if (course == null) return null;

        _mapper.Map(request.Dto, course);
        course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Courses.GetCourseWithDetailsAsync(request.Id);
        return _mapper.Map<CourseDto>(updated ?? course);
    }
}
