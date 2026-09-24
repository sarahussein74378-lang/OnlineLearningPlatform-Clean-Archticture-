using Mapster;
using OnlineLearningPlatform.Application.DTOs.Certificate;
using OnlineLearningPlatform.Application.DTOs.Course;
using OnlineLearningPlatform.Application.DTOs.Enrollment;
using OnlineLearningPlatform.Application.DTOs.Lesson;
using OnlineLearningPlatform.Application.DTOs.Progress;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using OnlineLearningPlatform.Domain.Entities;

namespace OnlineLearningPlatform.Application.Mapping;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateCourseDto, Course>.NewConfig();

        TypeAdapterConfig<UpdateCourseDto, Course>.NewConfig();

        TypeAdapterConfig<Course, CourseDto>.NewConfig()
            .Map(
                dest => dest.InstructorName,
                src => src.Instructor != null && src.Instructor.User != null
                    ? src.Instructor.User.FirstName + " " + src.Instructor.User.LastName
                    : string.Empty
            )
            .Map(
                dest => dest.TotalLessons,
                src => src.Lessons.Count
            )
            .Map(
                dest => dest.TotalEnrollments,
                src => src.Enrollments.Count
            );

        TypeAdapterConfig<CreateLessonDto, Lesson>.NewConfig();

        TypeAdapterConfig<Lesson, LessonDto>.NewConfig();

        TypeAdapterConfig<Enrollment, EnrollmentDto>.NewConfig()
            .Map(
                dest => dest.CourseTitle,
                src => src.Course != null
                    ? src.Course.Title
                    : string.Empty
            )
            .Map(
                dest => dest.Status,
                src => src.Status.ToString()
            )
            .Ignore(dest => dest.ProgressPercentage);

        TypeAdapterConfig<LessonProgress, LessonProgressDto>.NewConfig()
            .Map(
                dest => dest.LessonTitle,
                src => src.Lesson != null
                    ? src.Lesson.Title
                    : string.Empty
            );

        TypeAdapterConfig<CreateQuizDto, Quiz>.NewConfig()
            .Map(
                dest => dest.Questions,
                src => src.Questions
            );

        TypeAdapterConfig<CreateQuestionDto, Question>.NewConfig()
            .Map(
                dest => dest.Answers,
                src => src.Answers
            );

        TypeAdapterConfig<CreateAnswerDto, Answer>.NewConfig();

        TypeAdapterConfig<Quiz, QuizDto>.NewConfig();

        TypeAdapterConfig<Question, QuestionDto>.NewConfig();

        TypeAdapterConfig<Answer, AnswerDto>.NewConfig();

        TypeAdapterConfig<Certificate, CertificateDto>.NewConfig()
            .Map(
                dest => dest.StudentName,
                src => src.Student != null && src.Student.User != null
                    ? src.Student.User.FirstName + " " + src.Student.User.LastName
                    : string.Empty
            )
            .Map(
                dest => dest.CourseTitle,
                src => src.Enrollment != null &&
                       src.Enrollment.Course != null
                    ? src.Enrollment.Course.Title
                    : string.Empty
            )
            .Map(
                dest => dest.InstructorName,
                src => src.Enrollment != null &&
                       src.Enrollment.Course != null &&
                       src.Enrollment.Course.Instructor != null &&
                       src.Enrollment.Course.Instructor.User != null
                    ? src.Enrollment.Course.Instructor.User.FirstName + " " +
                      src.Enrollment.Course.Instructor.User.LastName
                    : string.Empty
            );
    }
}