using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.DataAccessLayer.Entities;

public class Order
{
    // Maps OrderID as the MongoDB document identifier (_id).
    [BsonId]
    // Stores Guid as a readable string instead of binary UUID.
    [BsonRepresentation(BsonType.String)]
    public Guid OrderID { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid UserID { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalBill { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}
