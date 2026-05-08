using Ecommerce.Core.DTO;
using FluentValidation;

namespace Ecommerce.Core.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
