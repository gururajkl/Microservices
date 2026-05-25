namespace Ecommerce.BusinessLogicLayer.DTO;

public record OrderResponse(Guid OrderID, Guid UserID, DateTime OrderDate, decimal TotalBill, List<OrderItemResponse> OrderItems,
    string? PersonName, string? Email)
{
    public OrderResponse() : this(default, default, default, default, default!,
        default, default)
    { }
}
