namespace OnlineLearningPlatform.DAL.Entities;

public class QuizSubmissionAnswer
{
    public int Id { get; set; }
    public int QuizSubmissionId { get; set; }
    public int QuestionId { get; set; }
    public int SelectedAnswerId { get; set; }
    public bool IsCorrect { get; set; }

    public QuizSubmission QuizSubmission { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public Answer SelectedAnswer { get; set; } = null!;
}
