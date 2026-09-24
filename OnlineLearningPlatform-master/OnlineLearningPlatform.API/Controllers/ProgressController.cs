using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Progress;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;

    public ProgressController(IProgressService progressService)
        => _progressService = progressService;

    [HttpGet("{enrollmentId:int}")]
    public async Task<ActionResult<ApiResponse<ProgressDto>>> GetProgress(int enrollmentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var progress = await _progressService.GetProgressAsync(enrollmentId, userId);
        if (progress == null) return NotFound(ApiResponse<ProgressDto>.Fail("Enrollment not found."));
        return Ok(ApiResponse<ProgressDto>.Ok(progress));
    }

    [HttpPatch("lesson")]
    public async Task<ActionResult<ApiResponse>> UpdateLessonProgress([FromBody] UpdateLessonProgressDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var updated = await _progressService.UpdateLessonProgressAsync(dto, userId);
        if (!updated) return BadRequest(ApiResponse.Fail("Progress update failed. Enrollment not found."));
        return Ok(ApiResponse.Ok("Progress updated."));
    }
}
