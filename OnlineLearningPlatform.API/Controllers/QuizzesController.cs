using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Application.Features.Quizzes.Queries.GetByCourse;
using OnlineLearningPlatform.Application.Features.Quizzes.Queries.GetById;
using OnlineLearningPlatform.Application.Features.Quizzes.Commands.CreateQuiz;
using OnlineLearningPlatform.Application.Features.Quizzes.Commands.DeleteQuiz;
using OnlineLearningPlatform.Application.Features.Quizzes.Commands.SubmitQuiz;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateQuizDto> _createValidator;

    public QuizzesController(IMediator mediator, IValidator<CreateQuizDto> createValidator)
    {
        _mediator = mediator;
        _createValidator = createValidator;
    }

    [HttpGet("course/{courseId:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IEnumerable<QuizDto>>>> GetByCourse(int courseId)
    {
        var quizzes = await _mediator.Send(new GetQuizzesByCourseQuery(courseId));
        return Ok(ApiResponse<IEnumerable<QuizDto>>.Ok(quizzes));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<QuizDto>>> GetById(int id)
    {
        var quiz = await _mediator.Send(new GetQuizByIdQuery(id));
        if (quiz == null) return NotFound(ApiResponse<QuizDto>.Fail("Quiz not found."));
        return Ok(ApiResponse<QuizDto>.Ok(quiz));
    }

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse<QuizDto>>> Create([FromBody] CreateQuizDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<QuizDto>.Fail("Validation failed",
                validation.Errors.Select(e => e.ErrorMessage)));

        var quiz = await _mediator.Send(new CreateQuizCommand(dto));
        return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, ApiResponse<QuizDto>.Ok(quiz, "Quiz created."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteQuizCommand(id));
        if (!deleted) return NotFound(ApiResponse.Fail("Quiz not found."));
        return Ok(ApiResponse.Ok("Quiz deleted."));
    }

    [HttpPost("submit")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<ApiResponse<QuizResultDto>>> Submit([FromBody] SubmitQuizDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _mediator.Send(new SubmitQuizCommand(dto, userId));
        return Ok(ApiResponse<QuizResultDto>.Ok(result, "Quiz submitted."));
    }
}
