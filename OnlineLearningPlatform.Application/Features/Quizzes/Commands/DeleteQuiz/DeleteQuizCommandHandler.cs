using MediatR;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Commands.DeleteQuiz;

public class DeleteQuizCommandHandler : IRequestHandler<DeleteQuizCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuizCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdAsync(request.Id);
        if (quiz == null) return false;
        _unitOfWork.Quizzes.Delete(quiz);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
