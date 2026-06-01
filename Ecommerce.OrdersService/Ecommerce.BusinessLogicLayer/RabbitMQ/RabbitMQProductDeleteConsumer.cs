using Ecommerce.BusinessLogicLayer.RabbitMQ.MessageTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Ecommerce.BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductDeleteConsumer : IRabbitMQProductDeleteConsumer, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQProductDeleteConsumer> _logger;

    public RabbitMQProductDeleteConsumer(IConfiguration configuration, ILogger<RabbitMQProductDeleteConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;

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
        string routingKey = "product.delete";
        string queueName = "orders.product.delete.queue";

        // Getting the exchange name from the configuration.
        string exchangeName = _configuration["RABBITMQ_Products_Exchange"]!;

        // Create or reuse the exchange.
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);

        // Create or reuse the queue.
        _channel.QueueDeclare(queueName, true, false, false, null);

        // Bind the queue to the exchange with the routing key.
        _channel.QueueBind(queueName, exchangeName, routingKey);

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (sender, args) =>
        {
            byte[] body = args.Body.ToArray();
            string stringMessage = Encoding.UTF8.GetString(body);
            ProductDeleteMessage message = JsonSerializer.Deserialize<ProductDeleteMessage>(stringMessage)!;

            _logger.LogInformation("Received ProductDeleteMessage: ProductID={ProductID}, ProductName={ProductName}",
                message.ProductID, message.ProductName);
        };

        _channel.BasicConsume(queueName, true, consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
