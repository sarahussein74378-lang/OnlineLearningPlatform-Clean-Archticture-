using Microsoft.EntityFrameworkCore;
using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Lessons.Queries.GetByCourse;

public class GetLessonsByCourseQueryHandler : IRequestHandler<GetLessonsByCourseQuery, IEnumerable<LessonDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetLessonsByCourseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonDto>> Handle(GetLessonsByCourseQuery request, CancellationToken cancellationToken)
    {
        var lessons = await _unitOfWork.Lessons.Query()
            .Where(l => l.CourseId == request.CourseId)
            .OrderBy(l => l.OrderIndex)
            .ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<LessonDto>>(lessons);
    }
}
