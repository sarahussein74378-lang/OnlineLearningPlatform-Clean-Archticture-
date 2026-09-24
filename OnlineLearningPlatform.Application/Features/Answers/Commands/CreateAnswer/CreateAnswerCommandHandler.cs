using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, AnswerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAnswerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AnswerDto> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
    {
        var answer = _mapper.Map<Answer>(request.Dto);
        answer.QuestionId = request.QuestionId;
        await _unitOfWork.Answers.AddAsync(answer);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AnswerDto>(answer);
    }
}
