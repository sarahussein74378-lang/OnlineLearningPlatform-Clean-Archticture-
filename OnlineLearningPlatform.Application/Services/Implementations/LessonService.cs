//using AutoMapper;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;


//using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.Application.Interfaces.Services;

//using OnlineLearningPlatform.BLL.DTOs.Lesson;
using OnlineLearningPlatform.Domain.Entities;
//using OnlineLearningPlatform.DAL.Entities;
//using OnlineLearningPlatform.DAL.UnitOfWork;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LessonDto>> GetByCourseAsync(int courseId)
    {
        var lessons = await _unitOfWork.Lessons.Query()
            .Where(l => l.CourseId == courseId)
            .OrderBy(l => l.OrderIndex)
            .ToListAsync();
        return _mapper.Map<IEnumerable<LessonDto>>(lessons);
    }

    public async Task<LessonDto?> GetByIdAsync(int id)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        return lesson == null ? null : _mapper.Map<LessonDto>(lesson);
    }

    public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
    {
        var lesson = _mapper.Map<Lesson>(dto);
        await _unitOfWork.Lessons.AddAsync(lesson);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<LessonDto>(lesson);
    }

    public async Task<LessonDto?> UpdateAsync(int id, CreateLessonDto dto)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null) return null;

        _mapper.Map(dto, lesson);
        _unitOfWork.Lessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<LessonDto>(lesson);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null) return false;

        _unitOfWork.Lessons.Delete(lesson);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

    