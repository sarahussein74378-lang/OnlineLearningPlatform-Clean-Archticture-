using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Quizzes.Queries.GetByCourse;

public class GetQuizzesByCourseQueryHandler : IRequestHandler<GetQuizzesByCourseQuery, IEnumerable<QuizDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuizzesByCourseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<QuizDto>> Handle(GetQuizzesByCourseQuery request, CancellationToken cancellationToken)
    {
        var quizzes = await _unitOfWork.Quizzes.GetQuizzesByCourseIdAsync(request.CourseId);
        return _mapper.Map<IEnumerable<QuizDto>>(quizzes);
    }
}
