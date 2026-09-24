namespace OnlineLearningPlatform.Application.DTOs.Quiz;

public class CreateQuizDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public int PassingScore { get; set; }
    public int TimeLimitMinutes { get; set; }
    public IEnumerable<CreateQuestionDto> Questions { get; set; } = [];
}

public class CreateQuestionDto
{
    public string Text { get; set; } = string.Empty;
    public int Points { get; set; } = 1;
    public int OrderIndex { get; set; }
    public IEnumerable<CreateAnswerDto> Answers { get; set; } = [];
}

public class CreateAnswerDto
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}

public class QuizDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CourseId { get; set; }
    public int PassingScore { get; set; }
    public int TimeLimitMinutes { get; set; }
    public IEnumerable<QuestionDto> Questions { get; set; } = [];
}

public class QuestionDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Points { get; set; }
    public int OrderIndex { get; set; }
    public IEnumerable<AnswerDto> Answers { get; set; } = [];
}

public class AnswerDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SubmitQuizDto
{
    public int QuizId { get; set; }
    public IEnumerable<QuizAnswerSubmissionDto> Answers { get; set; } = [];
}

public class QuizAnswerSubmissionDto
{
    public int QuestionId { get; set; }
    public int SelectedAnswerId { get; set; }
}

public class QuizResultDto
{
    public int QuizId { get; set; }
    public string QuizTitle { get; set; } = string.Empty;
    public int Score { get; set; }
    public int TotalPoints { get; set; }
    public int PassingScore { get; set; }
    public bool IsPassed { get; set; }
    public double Percentage { get; set; }
    public IEnumerable<QuestionResultDto> QuestionResults { get; set; } = [];
}

public class QuestionResultDto
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int SelectedAnswerId { get; set; }
    public int CorrectAnswerId { get; set; }
    public bool IsCorrect { get; set; }
}
