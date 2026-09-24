using MediatR;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Lessons.Commands.DeleteLesson;

public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLessonCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(request.Id);
        if (lesson == null) return false;

        _unitOfWork.Lessons.Delete(lesson);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
