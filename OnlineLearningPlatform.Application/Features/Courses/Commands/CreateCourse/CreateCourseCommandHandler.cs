//using AutoMapper;
using MapsterMapper;
using MediatR;
using OnlineLearningPlatform.Application.DTOs.Course;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.InstructorProfiles.GetByUserIdAsync(request.InstructorUserId);
        if (instructor == null)
            throw new KeyNotFoundException("Instructor profile not found.");

        var course = _mapper.Map<Course>(request.Dto);
        course.InstructorProfileId = instructor.Id;

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Courses.GetCourseWithDetailsAsync(course.Id);
        return _mapper.Map<CourseDto>(created ?? course);
    }
}
