using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Commands.CreateQuiz;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, QuizDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateQuizCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuizDto> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        var quiz = _mapper.Map<Quiz>(request.Dto);
        await _unitOfWork.Quizzes.AddAsync(quiz);
        await _unitOfWork.SaveChangesAsync();
        return (await _unitOfWork.Quizzes.GetQuizWithQuestionsAndAnswersAsync(quiz.Id)) is var created && created != null
            ? _mapper.Map<QuizDto>(created)
            : _mapper.Map<QuizDto>(quiz);
    }
}
