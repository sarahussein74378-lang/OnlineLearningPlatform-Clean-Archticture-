using OnlineLearningPlatform.Application.Interfaces;
using OnlineLearningPlatform.DAL.Data;
using OnlineLearningPlatform.DAL.Entities;
using OnlineLearningPlatform.DAL.Repositories.Implementations;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Courses = new GenericRepository<Course>(context);
        Lessons = new GenericRepository<Lesson>(context);
        Enrollments = new GenericRepository<Enrollment>(context);
        LessonProgresses = new GenericRepository<LessonProgress>(context);
        InstructorProfiles = new GenericRepository<InstructorProfile>(context);
        StudentProfiles = new GenericRepository<StudentProfile>(context);
        Quizzes = new GenericRepository<Quiz>(context);
        Questions = new GenericRepository<Question>(context);
        Answers = new GenericRepository<Answer>(context);
        QuizSubmissions = new GenericRepository<QuizSubmission>(context);
        QuizSubmissionAnswers = new GenericRepository<QuizSubmissionAnswer>(context);
        Certificates = new GenericRepository<Certificate>(context);
    }

    public IGenericRepository<Course> Courses { get; }
    public IGenericRepository<Lesson> Lessons { get; }
    public IGenericRepository<Enrollment> Enrollments { get; }
    public IGenericRepository<LessonProgress> LessonProgresses { get; }
    public IGenericRepository<InstructorProfile> InstructorProfiles { get; }
    public IGenericRepository<StudentProfile> StudentProfiles { get; }
    public IGenericRepository<Quiz> Quizzes { get; }
    public IGenericRepository<Question> Questions { get; }
    public IGenericRepository<Answer> Answers { get; }
    public IGenericRepository<QuizSubmission> QuizSubmissions { get; }
    public IGenericRepository<QuizSubmissionAnswer> QuizSubmissionAnswers { get; }
    public IGenericRepository<Certificate> Certificates { get; }

    Application.Interfaces.Repositories.ICourseRepository IUnitOfWork.Courses => throw new NotImplementedException();

    Application.Interfaces.Repositories.ILessonRepository IUnitOfWork.Lessons => throw new NotImplementedException();

    Application.Interfaces.Repositories.IEnrollmentRepository IUnitOfWork.Enrollments => throw new NotImplementedException();

    Application.Interfaces.Repositories.ILessonProgressRepository IUnitOfWork.LessonProgresses => throw new NotImplementedException();

    Application.Interfaces.Repositories.IInstructorProfileRepository IUnitOfWork.InstructorProfiles => throw new NotImplementedException();

    Application.Interfaces.Repositories.IStudentProfileRepository IUnitOfWork.StudentProfiles => throw new NotImplementedException();

    Application.Interfaces.Repositories.IQuizRepository IUnitOfWork.Quizzes => throw new NotImplementedException();

    Application.Interfaces.Repositories.IQuizSubmissionRepository IUnitOfWork.QuizSubmissions => throw new NotImplementedException();

    Application.Interfaces.Repositories.ICertificateRepository IUnitOfWork.Certificates => throw new NotImplementedException();

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
