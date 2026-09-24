using MediatR;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.InstructorProfiles.GetByUserIdAsync(request.InstructorUserId);
        if (instructor == null) return false;

        var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Id == request.Id && c.InstructorProfileId == instructor.Id);
        if (course == null) return false;

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
