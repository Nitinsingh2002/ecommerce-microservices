



using Authentication.Application.Features.Register;
using FluentValidation;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(X => X.LastName).NotEmpty().MaximumLength(100).MinimumLength(2);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(X => X.Password).NotEmpty()
            .MinimumLength(8).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.").
            Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.").
            Matches("[0-9]").WithMessage("Password must contain at least one digit.").
            Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

    }
}