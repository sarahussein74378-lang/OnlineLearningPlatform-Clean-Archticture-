using OnlineLearningPlatform.Application.DTOs.Auth;

namespace OnlineLearningPlatform.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto?> RegisterAsync(RegisterDto dto);
    Task<AuthResultDto?> LoginAsync(LoginDto dto);
}
