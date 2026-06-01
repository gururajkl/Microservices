using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Ecommerce.BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductNameUpdateConsumer : IDisposable, IRabbitMQProductNameUpdateConsumer
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQProductNameUpdateConsumer(IConfiguration configuration)
    {
        _configuration = configuration;

        string hostName = _configuration["RABBITMQ_HostName"]!;
        string port = _configuration["RABBITMQ_Port"]!;
        string userName = _configuration["RABBITMQ_UserName"]!;
        string password = _configuration["RABBITMQ_Password"]!;

        ConnectionFactory connectionFactory = new ConnectionFactory()
        {
            HostName = hostName,
            Port = int.Parse(port),
            UserName = userName,
            Password = password
        };

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Consume()
    {
        string queueName = "orders.product.update.name.queue";
        string routingKey = "product.update.name";

        // Getting the exchange name from the configuration.
        string exchangeName = _configuration["RABBITMQ_Products_Exchange"]!;

        // Create or reuse the exchange.
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);

        // Create or reuse the queue.
        _channel.QueueDeclare(queueName, true, false, false, null);

        // Bind the queue to the exchange with the routing key.
        _channel.QueueBind(queueName, exchangeName, routingKey);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
