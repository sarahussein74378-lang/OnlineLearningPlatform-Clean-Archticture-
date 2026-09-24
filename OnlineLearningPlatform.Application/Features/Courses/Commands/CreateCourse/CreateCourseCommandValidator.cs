using FluentValidation;

namespace OnlineLearningPlatform.Application.Features.Courses.Commands.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    private static readonly string[] AllowedLevels = ["Beginner", "Intermediate", "Advanced"];

    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Dto.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Dto.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Dto.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Dto.Level).NotEmpty().Must(l => AllowedLevels.Contains(l))
            .WithMessage("Level must be Beginner, Intermediate, or Advanced.");
        RuleFor(x => x.Dto.Category).NotEmpty().MaximumLength(100);
    }
}
