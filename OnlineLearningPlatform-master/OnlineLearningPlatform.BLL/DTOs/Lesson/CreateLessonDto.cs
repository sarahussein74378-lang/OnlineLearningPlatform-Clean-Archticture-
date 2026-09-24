namespace OnlineLearningPlatform.BLL.DTOs.Lesson;

public class CreateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public string? Content { get; set; }
    public int OrderIndex { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsFreePreview { get; set; }
    public int CourseId { get; set; }
}
