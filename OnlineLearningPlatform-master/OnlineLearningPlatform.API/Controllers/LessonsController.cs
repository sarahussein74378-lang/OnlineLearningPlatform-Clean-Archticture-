using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Lesson;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;
    private readonly IValidator<CreateLessonDto> _createValidator;

    public LessonsController(ILessonService lessonService, IValidator<CreateLessonDto> createValidator)
    {
        _lessonService = lessonService;
        _createValidator = createValidator;
    }

    [HttpGet("course/{courseId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LessonDto>>>> GetByCourse(int courseId)
    {
        var lessons = await _lessonService.GetByCourseAsync(courseId);
        return Ok(ApiResponse<IEnumerable<LessonDto>>.Ok(lessons));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<LessonDto>>> GetById(int id)
    {
        var lesson = await _lessonService.GetByIdAsync(id);
        if (lesson == null) return NotFound(ApiResponse<LessonDto>.Fail("Lesson not found."));
        return Ok(ApiResponse<LessonDto>.Ok(lesson));
    }

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<LessonDto>>> Create([FromBody] CreateLessonDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<LessonDto>.Fail("Validation failed",
                validation.Errors.Select(e => e.ErrorMessage)));

        var lesson = await _lessonService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, ApiResponse<LessonDto>.Ok(lesson, "Lesson created."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<LessonDto>>> Update(int id, [FromBody] CreateLessonDto dto)
    {
        var lesson = await _lessonService.UpdateAsync(id, dto);
        if (lesson == null) return NotFound(ApiResponse<LessonDto>.Fail("Lesson not found."));
        return Ok(ApiResponse<LessonDto>.Ok(lesson, "Lesson updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var deleted = await _lessonService.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse.Fail("Lesson not found."));
        return Ok(ApiResponse.Ok("Lesson deleted."));
    }
}
