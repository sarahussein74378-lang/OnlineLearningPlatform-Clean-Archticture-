namespace OnlineLearningPlatform.DAL.Entities;

public class Quiz
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public int PassingScore { get; set; }
    public int TimeLimitMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Course Course { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<QuizSubmission> Submissions { get; set; } = new List<QuizSubmission>();
}
