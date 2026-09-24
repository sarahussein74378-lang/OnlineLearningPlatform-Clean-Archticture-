using FluentValidation;
using OnlineLearningPlatform.BLL.DTOs.Course;

namespace OnlineLearningPlatform.BLL.Validators;

public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
    private static readonly string[] AllowedLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateCourseValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Level).NotEmpty().Must(l => AllowedLevels.Contains(l))
            .WithMessage("Level must be Beginner, Intermediate, or Advanced.");
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
    }
}
