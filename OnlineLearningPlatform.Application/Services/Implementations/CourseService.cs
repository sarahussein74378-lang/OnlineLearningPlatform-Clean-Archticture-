//using AutoMapper;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Course;
using OnlineLearningPlatform.Application.Interfaces;
//using OnlineLearningPlatform.BLL.Common;
//using OnlineLearningPlatform.BLL.DTOs.Course;
using OnlineLearningPlatform.BLL.Services.Interfaces;
using OnlineLearningPlatform.Domain.Entities;
//using OnlineLearningPlatform.DAL.Entities;
//using OnlineLearningPlatform.DAL.UnitOfWork;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<CourseDto>> GetAllAsync(int page, int pageSize, string? category, string? level)
    {
        var query = _unitOfWork.Courses.Query()
            .Include(c => c.Instructor).ThenInclude(i => i.User)
            .Include(c => c.Lessons)
            .Include(c => c.Enrollments)
            .Where(c => c.IsPublished);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(c => c.Category == category);
        if (!string.IsNullOrEmpty(level))
            query = query.Where(c => c.Level == level);

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        
       
        return new PaginatedResult<CourseDto>
        {
            Items = _mapper.Map<IEnumerable<CourseDto>>(items),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CourseDto?> GetByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses.Query()
            .Include(c => c.Instructor).ThenInclude(i => i.User)
            .Include(c => c.Lessons)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id);

        return course == null ? null : _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto dto, string instructorUserId)
    {
        var instructor = await _unitOfWork.InstructorProfiles
            .FindFirstAsync(i => i.UserId == instructorUserId);

        var course = _mapper.Map<Course>(dto);
        course.InstructorProfileId = instructor!.Id;

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return (await GetByIdAsync(course.Id))!;
    }

    public async Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto, string instructorUserId)
    {
        var instructor = await _unitOfWork.InstructorProfiles
            .FindFirstAsync(i => i.UserId == instructorUserId);

        var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Id == id && c.InstructorProfileId == instructor!.Id);
        if (course == null) return null;

        _mapper.Map(dto, course);
        course.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id, string instructorUserId)
    {
        var instructor = await _unitOfWork.InstructorProfiles
            .FindFirstAsync(i => i.UserId == instructorUserId);

        var course = await _unitOfWork.Courses.FindFirstAsync(c => c.Id == id && c.InstructorProfileId == instructor!.Id);
        if (course == null) return false;

        _unitOfWork.Courses.Delete(course);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CourseDto>> GetByInstructorAsync(string instructorUserId)
    {
        var courses = await _unitOfWork.Courses.Query()
            .Include(c => c.Instructor).ThenInclude(i => i.User)
            .Include(c => c.Lessons)
            .Include(c => c.Enrollments)
            .Where(c => c.Instructor.UserId == instructorUserId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    
}
