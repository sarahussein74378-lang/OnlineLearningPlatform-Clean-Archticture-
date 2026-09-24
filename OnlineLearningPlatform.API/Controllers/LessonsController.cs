using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.Features.Lessons.Queries.GetByCourse;
using OnlineLearningPlatform.Application.Features.Lessons.Queries.GetById;
using OnlineLearningPlatform.Application.Features.Lessons.Commands.CreateLesson;
using OnlineLearningPlatform.Application.Features.Lessons.Commands.UpdateLesson;
using OnlineLearningPlatform.Application.Features.Lessons.Commands.DeleteLesson;

//using OnlineLearningPlatform.BLL.Common;
//using OnlineLearningPlatform.BLL.DTOs.Lesson;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateLessonDto> _createValidator;

    public LessonsController(IMediator mediator, IValidator<CreateLessonDto> createValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
    }

    [HttpGet("course/{courseId:int}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LessonDto>>>> GetByCourse(int courseId)
    {
        var lessons = await _mediator.Send(new GetLessonsByCourseQuery(courseId));
        return Ok(ApiResponse<IEnumerable<LessonDto>>.Ok(lessons));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<LessonDto>>> GetById(int id)
    {
        var lesson = await _mediator.Send(new GetLessonByIdQuery(id));
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

        var lesson = await _mediator.Send(new CreateLessonCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, ApiResponse<LessonDto>.Ok(lesson, "Lesson created."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<LessonDto>>> Update(int id, [FromBody] CreateLessonDto dto)
    {
        var lesson = await _mediator.Send(new UpdateLessonCommand(id, dto));
        if (lesson == null) return NotFound(ApiResponse<LessonDto>.Fail("Lesson not found."));
        return Ok(ApiResponse<LessonDto>.Ok(lesson, "Lesson updated."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteLessonCommand(id));
        if (!deleted) return NotFound(ApiResponse.Fail("Lesson not found."));
        return Ok(ApiResponse.Ok("Lesson deleted."));
    }
}
