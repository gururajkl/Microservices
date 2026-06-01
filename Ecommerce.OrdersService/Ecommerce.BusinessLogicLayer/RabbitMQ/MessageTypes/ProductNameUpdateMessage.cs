namespace Ecommerce.BusinessLogicLayer.RabbitMQ.MessageTypes;

public record ProductNameUpdateMessage(Guid ProductID, string? NewProductName);
