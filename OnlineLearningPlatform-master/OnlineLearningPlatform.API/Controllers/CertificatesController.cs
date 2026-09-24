using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Certificate;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CertificatesController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificatesController(ICertificateService certificateService)
        => _certificateService = certificateService;

    [HttpGet("my")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CertificateDto>>>> GetMyCertificates()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var certs = await _certificateService.GetStudentCertificatesAsync(userId);
        return Ok(ApiResponse<IEnumerable<CertificateDto>>.Ok(certs));
    }

    [HttpGet("enrollment/{enrollmentId:int}")]
    public async Task<ActionResult<ApiResponse<CertificateDto>>> GetByEnrollment(int enrollmentId)
    {
        var cert = await _certificateService.GetByEnrollmentAsync(enrollmentId);
        if (cert == null) return NotFound(ApiResponse<CertificateDto>.Fail("Certificate not found."));
        return Ok(ApiResponse<CertificateDto>.Ok(cert));
    }

    [HttpPost("issue/{enrollmentId:int}")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<ActionResult<ApiResponse<CertificateDto>>> Issue(int enrollmentId)
    {
        var cert = await _certificateService.IssueCertificateAsync(enrollmentId);
        if (cert == null)
            return BadRequest(ApiResponse<CertificateDto>.Fail("Cannot issue certificate. Course not completed."));
        return Ok(ApiResponse<CertificateDto>.Ok(cert, "Certificate issued."));
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
        var pdfBytes = await _certificateService.GeneratePdfAsync(id);
        if (pdfBytes == null) return NotFound();
        return File(pdfBytes, "application/pdf", $"certificate-{id}.pdf");
    }
}
