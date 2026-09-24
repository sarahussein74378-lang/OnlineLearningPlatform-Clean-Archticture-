namespace OnlineLearningPlatform.Domain.Entities;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Points { get; set; } = 1;
    public int OrderIndex { get; set; }
    public int QuizId { get; set; }

    public Quiz Quiz { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}
