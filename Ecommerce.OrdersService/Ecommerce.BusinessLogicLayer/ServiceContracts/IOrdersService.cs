using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.DataAccessLayer.Entities;
using MongoDB.Driver;

namespace Ecommerce.BusinessLogicLayer.ServiceContracts;

public interface IOrdersService
{
    Task<List<OrderResponse?>> GetOrdersAsync();
    Task<List<OrderResponse?>> GetOrdersByConditionAsync(FilterDefinition<Order> filterDefinition);
    Task<OrderResponse?> GetOrderByConditionAsync(FilterDefinition<Order> filterDefinition);
    Task<OrderResponse?> AddOrderAsync(OrderAddRequest request);
    Task<OrderResponse?> UpdateOrderAsync(OrderUpdateRequest request);
    Task<bool> DeleteOrderAsync(Guid orderID);
}
