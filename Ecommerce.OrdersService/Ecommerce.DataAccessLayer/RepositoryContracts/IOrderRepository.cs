using Ecommerce.DataAccessLayer.Entities;
using MongoDB.Driver;

namespace Ecommerce.DataAccessLayer.RepositoryContracts;

/// <summary>
/// Defines data access operations for managing <see cref="Order"/> in MongoDB.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Returns all orders from the database.
    /// </summary>
    Task<IEnumerable<Order>> GetOrdersAsync();

    /// <summary>
    /// Returns all orders matching the given filter condition.
    /// </summary>
    /// <param name="filter">MongoDB filter to query orders (example by UserID, date range).</param>
    Task<IEnumerable<Order?>> GetOrdersByConditionAsync(FilterDefinition<Order> filter);

    /// <summary>
    /// Returns a single order matching the given filter condition.
    /// </summary>
    /// <param name="filter">MongoDB filter to find a specific order (example by OrderID).</param>
    Task<Order?> GetOrderByConditionAsync(FilterDefinition<Order> filter);

    /// <summary>
    /// Inserts a new order into the database.
    /// </summary>
    /// <param name="order">The order object to insert.</param>
    Task<Order?> AddOrderAsync(Order order);

    /// <summary>
    /// Updates an existing order in the database.
    /// </summary>
    /// <param name="order">The order object with updated values.</param>
    Task<Order?> UpdateOrderAsync(Order order);

    /// <summary>
    /// Deletes an order by its unique identifier.
    /// </summary>
    /// <param name="orderID">The unique ID of the order to delete.</param>
    Task<bool> DeleteOrderAsync(Guid orderID);
}