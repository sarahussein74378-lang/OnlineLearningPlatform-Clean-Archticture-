namespace OnlineLearningPlatform.Application.DTOs.Profile;

public class StudentProfileDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime EnrolledSince { get; set; }
}

public class InstructorProfileDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? Expertise { get; set; }
    public decimal Rating { get; set; }
    public int TotalStudents { get; set; }
}
