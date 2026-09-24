using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Enrollment;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
        => _enrollmentService = enrollmentService;

    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentDto>>>> GetMyEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(userId);
        return Ok(ApiResponse<IEnumerable<EnrollmentDto>>.Ok(enrollments));
    }

    [HttpPost("{courseId:int}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> Enroll(int courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollment = await _enrollmentService.EnrollAsync(courseId, userId);
        if (enrollment == null)
            return BadRequest(ApiResponse<EnrollmentDto>.Fail("Enrollment failed. Already enrolled or course not found."));
        return Ok(ApiResponse<EnrollmentDto>.Ok(enrollment, "Enrolled successfully."));
    }

    [HttpPatch("{enrollmentId:int}/drop")]
    public async Task<ActionResult<ApiResponse>> Drop(int enrollmentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var dropped = await _enrollmentService.DropCourseAsync(enrollmentId, userId);
        if (!dropped) return NotFound(ApiResponse.Fail("Enrollment not found."));
        return Ok(ApiResponse.Ok("Course dropped."));
    }
}
