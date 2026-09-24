using MediatR;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Questions.GetByIdAsync(request.Id);
        if (question == null) return false;

        _unitOfWork.Questions.Delete(question);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
