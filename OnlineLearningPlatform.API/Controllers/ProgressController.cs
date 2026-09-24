using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Progress;
using OnlineLearningPlatform.Application.Features.Progress.Queries.GetProgress;
using OnlineLearningPlatform.Application.Features.Progress.Commands.UpdateLessonProgress;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class ProgressController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgressController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{enrollmentId:int}")]
    public async Task<ActionResult<ApiResponse<ProgressDto>>> GetProgress(int enrollmentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var progress = await _mediator.Send(new GetProgressQuery(enrollmentId, userId));
        if (progress == null) return NotFound(ApiResponse<ProgressDto>.Fail("Enrollment not found."));
        return Ok(ApiResponse<ProgressDto>.Ok(progress));
    }

    [HttpPatch("lesson")]
    public async Task<ActionResult<ApiResponse>> UpdateLessonProgress([FromBody] UpdateLessonProgressDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var updated = await _mediator.Send(new UpdateLessonProgressCommand(dto, userId));
        if (!updated) return BadRequest(ApiResponse.Fail("Progress update failed. Enrollment not found."));
        return Ok(ApiResponse.Ok("Progress updated."));
    }
}
