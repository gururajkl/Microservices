using Ecommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace Ecommerce.BusinessLogicLayer.Validators;

public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
{
    public OrderUpdateRequestValidator()
    {
        RuleFor(r => r.UserID).NotEmpty().WithMessage("User ID is required");
        RuleFor(r => r.OrderID).NotEmpty().WithMessage("Order ID is required");
        RuleFor(r => r.OrderDate).NotEmpty().WithMessage("Order Date is required");
        RuleFor(r => r.OrderItems).NotEmpty().WithMessage("Order Items cannot be empty");
    }
}
