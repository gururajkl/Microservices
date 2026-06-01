namespace Ecommerce.BusinessLogicLayer.RabbitMQ.MessageTypes;

public record ProductUpdateMessage(Guid ProductID, string? ProductName, string Category, double? UnitPrice, int? QuantityInStock);
