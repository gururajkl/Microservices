using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrdersService service) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<OrderResponse?>> GetOrders()
    {
        return await service.GetOrdersAsync();
    }

    [HttpGet("search/order-id/{orderID}")]
    public async Task<OrderResponse?> GetOrderByOrderID(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderID);
        return await service.GetOrderByConditionAsync(filter);
    }

    [HttpGet("search/product-id/{productID}")]
    public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid productID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(o => o.OrderItems,
            Builders<OrderItem>.Filter.Eq(ot => ot.ProductID, productID));
        return await service.GetOrdersByConditionAsync(filter);
    }

    [HttpGet("search/orderDate/{orderDate}")]
    public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderDate.ToString("yyyyy-MM-dddd"), orderDate.ToString("yyyyy-MM-dddd"));
        return await service.GetOrdersByConditionAsync(filter);
    }
}
