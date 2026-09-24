using OnlineLearningPlatform.BLL.DTOs.Auth;

namespace OnlineLearningPlatform.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto?> RegisterAsync(RegisterDto dto);
    Task<AuthResultDto?> LoginAsync(LoginDto dto);
}
