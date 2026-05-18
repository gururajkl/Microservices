using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.DataAccessLayer.Entities;
using Ecommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;

namespace Ecommerce.BusinessLogicLayer.Services;

internal class OrderService(IOrderRepository repository, IMapper mapper,
    IValidator<OrderAddRequest> orderAddRequestValidator, IValidator<OrderItemAddRequest> orderItemAddValidator,
    IValidator<OrderUpdateRequest> orderUpdateRequestValidator, IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator) : IOrdersService
{
    public async Task<OrderResponse?> AddOrderAsync(OrderAddRequest request)
    {
        // If the parameter is null.
        ArgumentNullException.ThrowIfNull(request);

        // Validate Order using Fluent validation.
        ValidationResult orderValidationResult = await orderAddRequestValidator.ValidateAsync(request);

        if (!orderValidationResult.IsValid)
        {
            string errors = string.Join(", ", orderValidationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }

        // Validate OrderItems using Fluent validation.
        foreach (var orderItem in request.OrderItems)
        {
            ValidationResult orderItemValidationResult = await orderItemAddValidator.ValidateAsync(orderItem);

            if (!orderItemValidationResult.IsValid)
            {
                string errors = string.Join(", ", orderValidationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        // TODO: Validate userID using UsersMicroservice.

        Order orderToAdd = mapper.Map<Order>(request);

        // Calculate the total price.
        foreach (var orderItem in orderToAdd.OrderItems)
        {
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
        }

        orderToAdd.TotalBill = orderToAdd.OrderItems.Sum(o => o.TotalPrice);

        // Using repo add the order.
        Order? orderFromDB = await repository.AddOrderAsync(orderToAdd);

        if (orderFromDB is null) return null;

        return mapper.Map<OrderResponse>(orderFromDB);
    }

    public Task<bool> DeleteOrderAsync(Guid orderID)
    {
        throw new NotImplementedException();
    }

    public Task<OrderResponse?> GetOrderByConditionAsync(FilterDefinition<Order> filterDefinition)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderResponse?>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderResponse?>> GetOrdersByConditionAsync(FilterDefinition<Order> filterDefinition)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderResponse?> UpdateOrderAsync(OrderUpdateRequest request)
    {
        // If the parameter is null.
        ArgumentNullException.ThrowIfNull(request);

        // Validate Order using Fluent validation.
        ValidationResult orderUpdateValidationResult = await orderUpdateRequestValidator.ValidateAsync(request);

        if (!orderUpdateValidationResult.IsValid)
        {
            string errors = string.Join(", ", orderUpdateValidationResult.Errors.Select(e => e.ErrorMessage));
            throw new ArgumentException(errors);
        }

        // Validate OrderItems using Fluent validation.
        foreach (var orderItem in request.OrderItems)
        {
            ValidationResult orderItemUpdateValidationResult = await orderItemUpdateRequestValidator.ValidateAsync(orderItem);

            if (!orderItemUpdateValidationResult.IsValid)
            {
                string errors = string.Join(", ", orderUpdateValidationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        // TODO: Validate userID using UsersMicroservice.

        Order orderToUpdate = mapper.Map<Order>(request);

        // Calculate the total price.
        foreach (var orderItem in orderToUpdate.OrderItems)
        {
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
        }

        orderToUpdate.TotalBill = orderToUpdate.OrderItems.Sum(o => o.TotalPrice);

        // Using repo add the order.
        Order? orderFromDB = await repository.UpdateOrderAsync(orderToUpdate);

        if (orderFromDB is null) return null;

        return mapper.Map<OrderResponse>(orderFromDB);
    }
}
