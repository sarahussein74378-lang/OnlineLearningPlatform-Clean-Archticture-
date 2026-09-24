using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerCommandHandler : IRequestHandler<UpdateAnswerCommand, AnswerDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AnswerDto?> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = await _unitOfWork.Answers.GetByIdAsync(request.Id);
        if (answer == null) return null;

        _mapper.Map(request.Dto, answer);
        _unitOfWork.Answers.Update(answer);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AnswerDto>(answer);
    }
}
