using Microsoft.EntityFrameworkCore;
using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Questions.Queries.GetById;

public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetQuestionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<QuestionDto?> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _unitOfWork.Questions.Query()
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        return question == null ? null : _mapper.Map<QuestionDto>(question);
    }
}
