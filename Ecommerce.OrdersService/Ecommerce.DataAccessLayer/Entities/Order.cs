using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ecommerce.DataAccessLayer.Entities;

public class Order
{
    // With the help of this attribute OrderID will be used
    // as _id in MongoDb.
    [BsonId]
    // With the help of this attribute GUID will not be convereted as UUID
    // and will be stored as string and will be easier to read.
    [BsonRepresentation(BsonType.String)]
    public Guid OrderID { get; set; }
    [BsonRepresentation(BsonType.String)]
    public Guid UserID { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalBill { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
}
