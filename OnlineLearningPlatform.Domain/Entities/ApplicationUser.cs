using Microsoft.AspNetCore.Identity;

namespace OnlineLearningPlatform.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public InstructorProfile? InstructorProfile { get; set; }
    public StudentProfile? StudentProfile { get; set; }
}
