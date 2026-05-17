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

    public async Task<bool> DeleteOrderAsync(Guid orderID)
    {
        // Create filter as per the need.
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderID);

        // Pass the filter to find based on the condition.
        Order? orderToDelete = await (await _orders.FindAsync(filter)).FirstOrDefaultAsync();

        if (orderToDelete is null) return false;

        DeleteResult deleteResult = await _orders.DeleteOneAsync(filter);

        return deleteResult.DeletedCount > 0;
    }

    public async Task<Order?> GetOrderByConditionAsync(FilterDefinition<Order> filter)
    {
        return await (await _orders.FindAsync(filter)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        // Create empty filter.
        FilterDefinition<Order> filter = Builders<Order>.Filter.Empty;
        var orders = await _orders.FindAsync(filter);
        return await orders.ToListAsync();
    }

    public async Task<IEnumerable<Order?>> GetOrdersByConditionAsync(FilterDefinition<Order> filter)
    {
        return await (await _orders.FindAsync(filter)).ToListAsync();
    }

    public Task<Order?> UpdateOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
