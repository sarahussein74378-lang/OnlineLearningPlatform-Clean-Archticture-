using MapsterMapper;
using MediatR;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Course;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.Application.Features.Courses.Queries.GetAllCourses;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, PaginatedResult<CourseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<CourseDto>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _unitOfWork.Courses.GetPublishedCoursesPagedAsync(
            request.Page, request.PageSize, request.Category, request.Level);

        return new PaginatedResult<CourseDto>
        {
            Items = _mapper.Map<IEnumerable<CourseDto>>(items),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
