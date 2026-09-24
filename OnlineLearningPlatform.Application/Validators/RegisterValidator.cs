using FluentValidation;
using OnlineLearningPlatform.Application.DTOs.Auth;

namespace OnlineLearningPlatform.BLL.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    private static readonly string[] AllowedRoles = ["Instructor", "Student"];

    public RegisterValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        RuleFor(x => x.Role).NotEmpty().Must(r => AllowedRoles.Contains(r))
            .WithMessage("Role must be 'Instructor' or 'Student'.");
    }
}
