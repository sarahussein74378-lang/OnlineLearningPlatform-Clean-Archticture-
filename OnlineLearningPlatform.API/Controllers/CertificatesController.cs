using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.Application.Common;
using OnlineLearningPlatform.Application.DTOs.Certificate;
using MediatR;
using OnlineLearningPlatform.Application.Features.Certificates.Queries.GetStudentCertificates;
using OnlineLearningPlatform.Application.Features.Certificates.Queries.GetByEnrollment;
using OnlineLearningPlatform.Application.Features.Certificates.Commands.IssueCertificate;
using OnlineLearningPlatform.Application.Features.Certificates.Queries.GeneratePdf;
using OnlineLearningPlatform.Application.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CertificatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CertificatesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("my")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CertificateDto>>>> GetMyCertificates()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var certs = await _mediator.Send(new GetStudentCertificatesQuery(userId));
        return Ok(ApiResponse<IEnumerable<CertificateDto>>.Ok(certs));
    }

    [HttpGet("enrollment/{enrollmentId:int}")]
    public async Task<ActionResult<ApiResponse<CertificateDto>>> GetByEnrollment(int enrollmentId)
    {
        var cert = await _mediator.Send(new GetCertificateByEnrollmentQuery(enrollmentId));
        if (cert == null) return NotFound(ApiResponse<CertificateDto>.Fail("Certificate not found."));
        return Ok(ApiResponse<CertificateDto>.Ok(cert));
    }

    [HttpPost("issue/{enrollmentId:int}")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<ActionResult<ApiResponse<CertificateDto>>> Issue(int enrollmentId)
    {
        var cert = await _mediator.Send(new IssueCertificateCommand(enrollmentId));
        if (cert == null)
            return BadRequest(ApiResponse<CertificateDto>.Fail("Cannot issue certificate. Course not completed."));
        return Ok(ApiResponse<CertificateDto>.Ok(cert, "Certificate issued."));
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var pdfBytes = await _mediator.Send(new GenerateCertificatePdfQuery(id));
        if (pdfBytes == null) return NotFound();
        return File(pdfBytes, "application/pdf", $"certificate-{id}.pdf");
    }
}
