namespace OnlineLearningPlatform.BLL.DTOs.Progress;

public class ProgressDto
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public int CompletedLessons { get; set; }
    public int TotalLessons { get; set; }
    public double ProgressPercentage { get; set; }
    public IEnumerable<LessonProgressDto> LessonProgresses { get; set; } = [];
}

public class LessonProgressDto
{
    public int LessonId { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int WatchedSeconds { get; set; }
}

public class UpdateLessonProgressDto
{
    public int LessonId { get; set; }
    public bool IsCompleted { get; set; }
    public int WatchedSeconds { get; set; }
}
