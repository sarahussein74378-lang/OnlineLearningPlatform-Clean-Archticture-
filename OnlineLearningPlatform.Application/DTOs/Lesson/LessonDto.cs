namespace OnlineLearningPlatform.Application.DTOs.Lesson;

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string? Content { get; set; }
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsFreePreview { get; set; }
    public int CourseId { get; set; }
}
