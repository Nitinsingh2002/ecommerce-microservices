
using System.Data;
using Authentication.Application.Features.Login;
using FluentValidation;
namespace Authentication.Application.Features.LoginCommandValidator;


public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(X => X.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().MaximumLength(256);
        RuleFor(X => X.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.").Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.").Matches("[0-9]").WithMessage("Password must contain at least one digit.").Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}