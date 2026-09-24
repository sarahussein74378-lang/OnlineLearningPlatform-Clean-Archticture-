using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Quiz;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;
    private readonly IValidator<CreateQuizDto> _createValidator;

    public QuizzesController(IQuizService quizService, IValidator<CreateQuizDto> createValidator)
    {
        _quizService = quizService;
        _createValidator = createValidator;
    }

    [HttpGet("course/{courseId:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IEnumerable<QuizDto>>>> GetByCourse(int courseId)
    {
        var quizzes = await _quizService.GetByCourseAsync(courseId);
        return Ok(ApiResponse<IEnumerable<QuizDto>>.Ok(quizzes));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<QuizDto>>> GetById(int id)
    {
        var quiz = await _quizService.GetByIdAsync(id);
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

        var quiz = await _quizService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, ApiResponse<QuizDto>.Ok(quiz, "Quiz created."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Instructor")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var deleted = await _quizService.DeleteAsync(id);
        if (!deleted) return NotFound(ApiResponse.Fail("Quiz not found."));
        return Ok(ApiResponse.Ok("Quiz deleted."));
    }

    [HttpPost("submit")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<ApiResponse<QuizResultDto>>> Submit([FromBody] SubmitQuizDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _quizService.SubmitAsync(dto, userId);
        return Ok(ApiResponse<QuizResultDto>.Ok(result, "Quiz submitted."));
    }
}
