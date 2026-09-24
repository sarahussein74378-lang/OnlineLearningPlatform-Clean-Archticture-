using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using OnlineLearningPlatform.BLL.Common;
using OnlineLearningPlatform.BLL.DTOs.Auth;
using OnlineLearningPlatform.BLL.Services.Interfaces;

namespace OnlineLearningPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterDto> _registerValidator;

    public AuthController(IAuthService authService, IValidator<RegisterDto> registerValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> Register([FromBody] RegisterDto dto)
    {
        var validation = await _registerValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<AuthResultDto>.Fail("Validation failed",
                validation.Errors.Select(e => e.ErrorMessage)));

        var result = await _authService.RegisterAsync(dto);
        if (result == null)
            return BadRequest(ApiResponse<AuthResultDto>.Fail("Registration failed. Email may already be in use."));

        return Ok(ApiResponse<AuthResultDto>.Ok(result, "Registration successful."));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(ApiResponse<AuthResultDto>.Fail("Invalid email or password."));

        return Ok(ApiResponse<AuthResultDto>.Ok(result, "Login successful."));
    }
}
