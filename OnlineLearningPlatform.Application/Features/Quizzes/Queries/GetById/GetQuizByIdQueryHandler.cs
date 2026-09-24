using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Queries.GetById;

public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, QuizDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuizByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuizDto?> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
    {
        var quiz = await _unitOfWork.Quizzes.GetQuizWithQuestionsAndAnswersAsync(request.Id);
        return quiz == null ? null : _mapper.Map<QuizDto>(quiz);
    }
}
