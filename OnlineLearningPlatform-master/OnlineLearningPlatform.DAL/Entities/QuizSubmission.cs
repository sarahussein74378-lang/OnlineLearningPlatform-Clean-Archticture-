namespace OnlineLearningPlatform.DAL.Entities;

public class QuizSubmission
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public int StudentProfileId { get; set; }
    public int Score { get; set; }
    public int TotalPoints { get; set; }
    public bool IsPassed { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public Quiz Quiz { get; set; } = null!;
    public StudentProfile Student { get; set; } = null!;
    public ICollection<QuizSubmissionAnswer> SubmissionAnswers { get; set; } = new List<QuizSubmissionAnswer>();
}
