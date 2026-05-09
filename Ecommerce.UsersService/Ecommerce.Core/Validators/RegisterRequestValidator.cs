using Ecommerce.Core.DTO;
using FluentValidation;

namespace Ecommerce.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(r => r.PersonName)
            .NotEmpty().WithMessage("Person name is required")
            .MaximumLength(50).WithMessage("Person name must not exceed 50 characters");

        RuleFor(r => r.Gender).IsInEnum().WithMessage("Invalid gender option");
    }
}
