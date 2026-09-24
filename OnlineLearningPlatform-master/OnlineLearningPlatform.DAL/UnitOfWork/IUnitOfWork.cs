using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.Repositories.Interfaces;

namespace OnlineLearningPlatform.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Lesson> Lessons { get; }
    IGenericRepository<Enrollment> Enrollments { get; }
    IGenericRepository<LessonProgress> LessonProgresses { get; }
    IGenericRepository<InstructorProfile> InstructorProfiles { get; }
    IGenericRepository<StudentProfile> StudentProfiles { get; }
    IGenericRepository<Quiz> Quizzes { get; }
    IGenericRepository<Question> Questions { get; }
    IGenericRepository<Answer> Answers { get; }
    IGenericRepository<QuizSubmission> QuizSubmissions { get; }
    IGenericRepository<QuizSubmissionAnswer> QuizSubmissionAnswers { get; }
    IGenericRepository<Certificate> Certificates { get; }

    Task<int> SaveChangesAsync();
}
