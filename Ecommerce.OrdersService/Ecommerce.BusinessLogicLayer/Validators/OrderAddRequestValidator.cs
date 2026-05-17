using Ecommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace Ecommerce.BusinessLogicLayer.Validators;

public class OrderAddRequestValidator : AbstractValidator<OrderAddRequest>
{
    public OrderAddRequestValidator()
    {
        RuleFor(r => r.UserID).NotEmpty().WithMessage("User ID is required");
        RuleFor(r => r.OrderDate).NotEmpty().WithMessage("Order Date is required");
        RuleFor(r => r.OrderItems).NotEmpty().WithMessage("Order Items cannot be empty");
    }
}
