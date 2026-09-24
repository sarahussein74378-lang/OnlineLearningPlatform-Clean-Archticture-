using OnlineLearningPlatform.Application.Interfaces.Repositories;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    ILessonRepository Lessons { get; }
    IEnrollmentRepository Enrollments { get; }
    ILessonProgressRepository LessonProgresses { get; }
    IInstructorProfileRepository InstructorProfiles { get; }
    IStudentProfileRepository StudentProfiles { get; }
    IQuizRepository Quizzes { get; }
    IGenericRepository<Question> Questions { get; }
    IGenericRepository<Answer> Answers { get; }
    IQuizSubmissionRepository QuizSubmissions { get; }
    IGenericRepository<QuizSubmissionAnswer> QuizSubmissionAnswers { get; }
    ICertificateRepository Certificates { get; }

    Task<int> SaveChangesAsync();
}