namespace Ecommerce.BusinessLogicLayer.RabbitMQ;

public interface IRabbitMQProductDeleteConsumer
{
    void Consume();
    void Dispose();
}
