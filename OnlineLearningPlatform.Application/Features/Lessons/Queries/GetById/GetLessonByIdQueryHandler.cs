using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Lessons.Queries.GetById;

public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, LessonDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLessonByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LessonDto?> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(request.Id);
        return lesson == null ? null : _mapper.Map<LessonDto>(lesson);
    }
}
