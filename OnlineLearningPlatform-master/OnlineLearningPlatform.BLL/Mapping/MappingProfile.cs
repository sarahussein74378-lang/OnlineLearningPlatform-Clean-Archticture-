using AutoMapper;
using OnlineLearningPlatform.BLL.DTOs.Certificate;
using OnlineLearningPlatform.BLL.DTOs.Course;
using OnlineLearningPlatform.BLL.DTOs.Enrollment;
using OnlineLearningPlatform.BLL.DTOs.Lesson;
using OnlineLearningPlatform.BLL.DTOs.Progress;
using OnlineLearningPlatform.BLL.DTOs.Quiz;
using OnlineLearningPlatform.DAL.Entities;

namespace OnlineLearningPlatform.BLL.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCourseDto, Course>();    
        CreateMap<UpdateCourseDto, Course>();
        CreateMap<Course, CourseDto>()
            .ForMember(d => d.InstructorName, o => o.MapFrom(s =>
                s.Instructor != null ? s.Instructor.User.FirstName + " " + s.Instructor.User.LastName : string.Empty))
            .ForMember(d => d.TotalLessons, o => o.MapFrom(s => s.Lessons.Count))
            .ForMember(d => d.TotalEnrollments, o => o.MapFrom(s => s.Enrollments.Count));

        CreateMap<CreateLessonDto, Lesson>();
        CreateMap<Lesson, LessonDto>();

        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.ProgressPercentage, o => o.Ignore());

        CreateMap<LessonProgress, LessonProgressDto>()
            .ForMember(d => d.LessonTitle, o => o.MapFrom(s => s.Lesson.Title));

        CreateMap<CreateQuizDto, Quiz>()
            .ForMember(d => d.Questions, o => o.MapFrom(s => s.Questions));
        CreateMap<CreateQuestionDto, Question>()
            .ForMember(d => d.Answers, o => o.MapFrom(s => s.Answers));
        CreateMap<CreateAnswerDto, Answer>();

        CreateMap<Quiz, QuizDto>();
        CreateMap<Question, QuestionDto>();
        CreateMap<Answer, AnswerDto>();

        CreateMap<Certificate, CertificateDto>()
            .ForMember(d => d.StudentName, o => o.MapFrom(s =>
                s.Student.User.FirstName + " " + s.Student.User.LastName))
            .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Enrollment.Course.Title))
            .ForMember(d => d.InstructorName, o => o.MapFrom(s =>
                s.Enrollment.Course.Instructor.User.FirstName + " " + s.Enrollment.Course.Instructor.User.LastName));
    }
}
