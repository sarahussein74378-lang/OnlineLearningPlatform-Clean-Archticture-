using MediatR;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerCommandHandler : IRequestHandler<DeleteAnswerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnswerCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(DeleteAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await _unitOfWork.Answers.GetByIdAsync(request.Id);
        if (answer == null) return false;

        _unitOfWork.Answers.Delete(answer);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
