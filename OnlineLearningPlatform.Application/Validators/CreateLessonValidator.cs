using FluentValidation;
using OnlineLearningPlatform.Application.DTOs.Lesson;


namespace OnlineLearningPlatform.BLL.Validators;

public class CreateLessonValidator : AbstractValidator<CreateLessonDto>
{
    public CreateLessonValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.DurationMinutes).GreaterThanOrEqualTo(0);
        RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
    }
}
