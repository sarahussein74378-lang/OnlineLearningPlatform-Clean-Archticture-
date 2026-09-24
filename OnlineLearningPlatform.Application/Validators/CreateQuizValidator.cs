using FluentValidation;
using OnlineLearningPlatform.Application.DTOs.Quiz;
//using OnlineLearningPlatform.BLL.DTOs.Quiz;

namespace OnlineLearningPlatform.BLL.Validators;

public class CreateQuizValidator : AbstractValidator<CreateQuizDto>
{
    public CreateQuizValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.CourseId).GreaterThan(0);
        RuleFor(x => x.PassingScore).InclusiveBetween(1, 100);
        RuleFor(x => x.TimeLimitMinutes).GreaterThan(0);
        RuleFor(x => x.Questions).NotEmpty().WithMessage("A quiz must have at least one question.");
        RuleForEach(x => x.Questions).ChildRules(q =>
        {
            q.RuleFor(x => x.Text).NotEmpty();
            q.RuleFor(x => x.Points).GreaterThan(0);
            q.RuleFor(x => x.Answers).Must(a => a.Count() >= 2)
                .WithMessage("Each question must have at least 2 answers.");
            q.RuleFor(x => x.Answers).Must(a => a.Count(x => x.IsCorrect) == 1)
                .WithMessage("Each question must have exactly one correct answer.");
        });
    }
}
