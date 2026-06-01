namespace Ecommerce.BusinessLogicLayer.RabbitMQ.MessageTypes;

public record ProductDeleteMessage(Guid ProductID, string ProductName);
