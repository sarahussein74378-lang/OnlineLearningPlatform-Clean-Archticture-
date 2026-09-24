using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineLearningPlatform.BLL.DTOs.Auth;
using OnlineLearningPlatform.BLL.Services.Interfaces;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.UnitOfWork;

namespace OnlineLearningPlatform.BLL.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResultDto?> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return null;

        await _userManager.AddToRoleAsync(user, dto.Role);

        if (dto.Role == "Instructor")
            await _unitOfWork.InstructorProfiles.AddAsync(new InstructorProfile { UserId = user.Id });
        else
            await _unitOfWork.StudentProfiles.AddAsync(new StudentProfile { UserId = user.Id });

        await _unitOfWork.SaveChangesAsync();

        return await BuildAuthResult(user, dto.Role);
    }

    public async Task<AuthResultDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return null;

        var roles = await _userManager.GetRolesAsync(user);
        return await BuildAuthResult(user, roles.FirstOrDefault() ?? string.Empty);
    }

    private Task<AuthResultDto> BuildAuthResult(ApplicationUser user, string role)
    {
        var jwtKey = _configuration["Jwt:Key"]!;
        var jwtIssuer = _configuration["Jwt:Issuer"]!;
        var jwtAudience = _configuration["Jwt:Audience"]!;
        var expiryHours = double.Parse(_configuration["Jwt:ExpiryHours"] ?? "24");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Role, role),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddHours(expiryHours);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expiry,
            signingCredentials: creds);

        return Task.FromResult(new AuthResultDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiry,
            UserId = user.Id,
            Email = user.Email!,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = role
        });
    }
}
