using MediatR;
using MapsterMapper;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Features.Lessons.Commands.UpdateLesson;

public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, LessonDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLessonCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LessonDto?> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(request.Id);
        if (lesson == null) return null;

        _mapper.Map(request.Dto, lesson);
        _unitOfWork.Lessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<LessonDto>(lesson);
    }
}
