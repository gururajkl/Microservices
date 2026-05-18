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
    public async Task<OrderResponse?> GetOrder(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(o => o.OrderID, orderID);
        return await service.GetOrderByConditionAsync(filter);
    }
}
