using Ecommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace Ecommerce.BusinessLogicLayer.Validators;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(p => p.ProductID).NotEmpty().WithMessage("ProductID cannot be empty.");
        RuleFor(p => p.ProductName).NotEmpty().WithMessage("Product name cannot be empty.");
        RuleFor(p => p.Category).IsInEnum().WithMessage("Invalid category passed.");
        RuleFor(p => p.UnitPrice).InclusiveBetween(0, double.MaxValue).WithMessage($"Unit price should be between 0 to {double.MaxValue}");
        RuleFor(p => p.QuantityInStock).InclusiveBetween(0, int.MaxValue).WithMessage($"Quantity should be between 0 to {int.MaxValue}");
    }
}