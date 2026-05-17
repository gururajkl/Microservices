using Ecommerce.DataAccessLayer.Entities;
using Ecommerce.DataAccessLayer.RepositoryContracts;
using MongoDB.Driver;

namespace Ecommerce.DataAccessLayer.Repositories;

internal class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _orders;

    public OrderRepository(IMongoDatabase mongoDatabase)
    {
        _orders = mongoDatabase.GetCollection<Order>("orders");
    }

    public async Task<Order?> AddOrderAsync(Order order)
    {
        order.OrderID = Guid.NewGuid();
        await _orders.InsertOneAsync(order);
        return order;
    }

    public Task<bool> DeleteOrderAsync(Guid orderID)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrderByConditionAsync(FilterDefinition<Order> filter)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order?>> GetOrdersByConditionAsync(FilterDefinition<Order> filter)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> UpdateOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
