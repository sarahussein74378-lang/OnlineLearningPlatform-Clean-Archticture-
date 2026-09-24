namespace OnlineLearningPlatform.BLL.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public decimal Price { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public int TotalLessons { get; set; }
    public int TotalEnrollments { get; set; }
}
