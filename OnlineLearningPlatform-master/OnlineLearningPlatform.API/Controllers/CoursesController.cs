using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Course;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IValidator<CreateCourseDto> _createValidator;

    public CoursesController(ICourseService courseService, IValidator<CreateCourseDto> createValidator)
    {
        _courseService = courseService;
        _createValidator = createValidator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<CourseDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null,
        [FromQuery] string? level = null)
    {
        var result = await _courseService.GetAllAsync(page, pageSize, category, level);
        return Ok(ApiResponse<PaginatedResult<CourseDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> GetById(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course == null) return NotFound(ApiResponse<CourseDto>.Fail("Course not found."));
        return Ok(ApiResponse<CourseDto>.Ok(course));
    }

    [HttpGet("my")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetMyCourses()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var courses = await _courseService.GetByInstructorAsync(userId);
        return Ok(ApiResponse<IEnumerable<CourseDto>>.Ok(courses));
    }

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> Create([FromBody] CreateCourseDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<CourseDto>.Fail("Validation failed",
                validation.Errors.Select(e => e.ErrorMessage)));

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var course = await _courseService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, ApiResponse<CourseDto>.Ok(course, "Course created."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<CourseDto>>> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var course = await _courseService.UpdateAsync(id, dto, userId);
        if (course == null) return NotFound(ApiResponse<CourseDto>.Fail("Course not found or access denied."));
        return Ok(ApiResponse<CourseDto>.Ok(course, "Course updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var deleted = await _courseService.DeleteAsync(id, userId);
        if (!deleted) return NotFound(ApiResponse.Fail("Course not found or access denied."));
        return Ok(ApiResponse.Ok("Course deleted."));
    }
}
