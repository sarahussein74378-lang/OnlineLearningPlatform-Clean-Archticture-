using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Enrollment;
using OnlineLearningPlatform.Application.Features.Enrollments.Queries.GetStudentEnrollments;
using OnlineLearningPlatform.Application.Features.Enrollments.Commands.Enroll;
using OnlineLearningPlatform.Application.Features.Enrollments.Commands.Drop;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Student")]
public class EnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollmentsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<IEnumerable<EnrollmentDto>>>> GetMyEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollments = await _mediator.Send(new GetStudentEnrollmentsQuery(userId));
        return Ok(ApiResponse<IEnumerable<EnrollmentDto>>.Ok(enrollments));
    }

    [HttpPost("{courseId:int}")]
    public async Task<ActionResult<ApiResponse<EnrollmentDto>>> Enroll(int courseId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollment = await _mediator.Send(new EnrollCommand(courseId, userId));
        if (enrollment == null)
            return BadRequest(ApiResponse<EnrollmentDto>.Fail("Enrollment failed. Already enrolled or course not found."));
        return Ok(ApiResponse<EnrollmentDto>.Ok(enrollment, "Enrolled successfully."));
    }

    [HttpPatch("{enrollmentId:int}/drop")]
    public async Task<ActionResult<ApiResponse>> Drop(int enrollmentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var dropped = await _mediator.Send(new DropEnrollmentCommand(enrollmentId, userId));
        if (!dropped) return NotFound(ApiResponse.Fail("Enrollment not found."));
        return Ok(ApiResponse.Ok("Course dropped."));
    }
}
