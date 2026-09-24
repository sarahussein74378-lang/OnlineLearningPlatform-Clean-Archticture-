using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.DAL.Data.Configurations;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.DAL.Data;

public class AppDbContext : IdentityDbContext<Entities.ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<InstructorProfile> InstructorProfiles { get; set; }
    public DbSet<StudentProfile> StudentProfiles { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<LessonProgress> LessonProgresses { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuizSubmission> QuizSubmissions { get; set; }
    public DbSet<QuizSubmissionAnswer> QuizSubmissionAnswers { get; set; }
    public DbSet<Certificate> Certificates { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(CourseConfiguration).Assembly);
    }
}
