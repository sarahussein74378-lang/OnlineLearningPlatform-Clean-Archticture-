namespace OnlineLearningPlatform.DAL.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int StudentProfileId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    public StudentProfile Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public Certificate? Certificate { get; set; }
}

public enum EnrollmentStatus
{
    Active,
    Completed,
    Dropped
}
