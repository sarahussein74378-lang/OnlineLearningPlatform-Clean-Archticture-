using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Features.Lessons.Commands.CreateLesson;

public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, LessonDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLessonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LessonDto> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = _mapper.Map<Lesson>(request.Dto);
        await _unitOfWork.Lessons.AddAsync(lesson);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<LessonDto>(lesson);
    }
}
