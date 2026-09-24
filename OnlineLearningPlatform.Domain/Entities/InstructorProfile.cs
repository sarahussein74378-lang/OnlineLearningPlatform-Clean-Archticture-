namespace OnlineLearningPlatform.Domain.Entities;

public class InstructorProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? Expertise { get; set; }
    public decimal Rating { get; set; }
    public int TotalStudents { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
