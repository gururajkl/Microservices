using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.HttpClients;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.DataAccessLayer.Entities;
using Ecommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;

namespace Ecommerce.BusinessLogicLayer.Services;

internal class OrderService(IOrderRepository repository, IMapper mapper,
    IValidator<OrderAddRequest> orderAddRequestValidator, IValidator<OrderItemAddRequest> orderItemAddValidator,
    IValidator<OrderUpdateRequest> orderUpdateRequestValidator, IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator,
    UsersMicroserviceClient usersServiceClient, ProductsMicroserviceClient productsServiceClient) : IOrdersService
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

        List<ProductDTO> products = [];

        // Validate OrderItems using Fluent validation.
        foreach (var orderItem in request.OrderItems)
        {
            ValidationResult orderItemValidationResult = await orderItemAddValidator.ValidateAsync(orderItem);

            if (!orderItemValidationResult.IsValid)
            {
                string errors = string.Join(", ", orderValidationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            // Validate the product id by calling the products microservice.
            ProductDTO? product = await productsServiceClient.GetProductByProductID(orderItem.ProductID)
                ?? throw new ArgumentException("Invalid product id");

            products.Add(product);
        }

        UserDTO? user = await usersServiceClient.GetUserByUserID(request.UserID) ?? throw new ArgumentException("Invalid user id");

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

        var orderResponse = mapper.Map<OrderResponse>(orderFromDB);

        if (orderResponse is null) return null;

        foreach (var orderItem in orderResponse.OrderItems)
        {
            ProductDTO? productDTO = products.FirstOrDefault(p => p.ProductID == orderItem.ProductID);
            if (productDTO is null) continue;
            mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItem);
        }

        return orderResponse;
    }

    public async Task<bool> DeleteOrderAsync(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderID);

        Order? order = await repository.GetOrderByConditionAsync(filter);

        if (order is null) return false;

        return await repository.DeleteOrderAsync(orderID);
    }

    public async Task<OrderResponse?> GetOrderByConditionAsync(FilterDefinition<Order> filterDefinition)
    {
        Order? order = await repository.GetOrderByConditionAsync(filterDefinition);

        if (order is null) return null;

        var orderResponse = mapper.Map<OrderResponse>(order);

        if (orderResponse is null) return null;

        foreach (var orderItem in orderResponse.OrderItems)
        {
            ProductDTO? productDTO = await productsServiceClient.GetProductByProductID(orderItem.ProductID);

            if (productDTO is null) continue;

            mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItem);
        }

        return orderResponse;
    }

    public async Task<List<OrderResponse?>> GetOrdersAsync()
    {
        var orders = await repository.GetOrdersAsync();
        IEnumerable<OrderResponse> orderResponses = mapper.Map<IEnumerable<OrderResponse>>(orders);

        foreach (var orderResponse in orderResponses)
        {
            if (orderResponse is null)
            {
                continue;
            }

            foreach (var orderItem in orderResponse.OrderItems)
            {
                ProductDTO? productDTO = await productsServiceClient.GetProductByProductID(orderItem.ProductID);

                if (productDTO is null) continue;

                mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItem);
            }
        }

        return [.. orderResponses];
    }

    public async Task<List<OrderResponse?>> GetOrdersByConditionAsync(FilterDefinition<Order> filterDefinition)
    {
        IEnumerable<Order?> orders = await repository.GetOrdersByConditionAsync(filterDefinition);

        if (orders is null) return [];

        var orderResponses = mapper.Map<IEnumerable<OrderResponse?>>(orders);

        foreach (var orderResponse in orderResponses)
        {
            if (orderResponse is null)
            {
                continue;
            }

            foreach (var orderItem in orderResponse.OrderItems)
            {
                ProductDTO? productDTO = await productsServiceClient.GetProductByProductID(orderItem.ProductID);

                if (productDTO is null) continue;

                mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItem);
            }
        }

        return [.. orderResponses];
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

        List<ProductDTO> products = [];

        // Validate OrderItems using Fluent validation.
        foreach (var orderItem in request.OrderItems)
        {
            ValidationResult orderItemUpdateValidationResult = await orderItemUpdateRequestValidator.ValidateAsync(orderItem);

            if (!orderItemUpdateValidationResult.IsValid)
            {
                string errors = string.Join(", ", orderUpdateValidationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            // Validate the product id by calling the products microservice.
            ProductDTO? product = await productsServiceClient.GetProductByProductID(orderItem.ProductID)
                ?? throw new ArgumentException("Invalid product id");

            products.Add(product);
        }

        UserDTO? user = await usersServiceClient.GetUserByUserID(request.UserID) ?? throw new ArgumentException("Invalid user id");

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

        var orderResponse = mapper.Map<OrderResponse>(orderFromDB);

        if (orderResponse is null) return null;

        foreach (var orderItem in orderResponse.OrderItems)
        {
            ProductDTO? productDTO = products.FirstOrDefault(p => p.ProductID == orderItem.ProductID);
            if (productDTO is null) continue;
            mapper.Map<ProductDTO, OrderItemResponse>(productDTO, orderItem);
        }

        return orderResponse;
    }
}
