using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuestionDto?> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Questions.GetByIdAsync(request.Id);
        if (question == null) return null;

        _mapper.Map(request.Dto, question);
        _unitOfWork.Questions.Update(question);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<QuestionDto>(question);
    }
}
