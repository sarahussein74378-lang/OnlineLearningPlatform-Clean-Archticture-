namespace OnlineLearningPlatform.Domain.Entities;

public class StudentProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime EnrolledSince { get; set; } = DateTime.UtcNow;

    public ApplicationUser User { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}
