using Ecommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace Ecommerce.BusinessLogicLayer.Validators;

public class OrderItemAddRequestValidator : AbstractValidator<OrderItemAddRequest>
{
    public OrderItemAddRequestValidator()
    {
        RuleFor(r => r.ProductID).NotEmpty().WithMessage("Product ID is required");
        RuleFor(r => r.UnitPrice).NotEmpty().WithMessage("Unit Price is required")
            .GreaterThan(0).WithMessage("Unit price should be greater than 0");
        RuleFor(r => r.Quantity).NotEmpty().WithMessage("Quantity cannot be empty")
            .GreaterThan(0).WithMessage("Quantity should be greater than 0");
    }
}
