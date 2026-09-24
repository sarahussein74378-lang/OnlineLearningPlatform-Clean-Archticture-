using OnlineLearningPlatform.Application.DTOs.Auth;

namespace OnlineLearningPlatform.Application.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResultDto?> RegisterAsync(RegisterDto dto);
    Task<AuthResultDto?> LoginAsync(LoginDto dto);
}
