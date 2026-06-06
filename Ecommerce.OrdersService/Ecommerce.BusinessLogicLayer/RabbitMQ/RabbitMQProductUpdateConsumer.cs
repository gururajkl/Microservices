using Ecommerce.BusinessLogicLayer.RabbitMQ.MessageTypes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Ecommerce.BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductUpdateConsumer : IDisposable, IRabbitMQProductNameUpdateConsumer
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQProductUpdateConsumer> _logger;
    private readonly IDistributedCache _cache;

    public RabbitMQProductUpdateConsumer(IConfiguration configuration, ILogger<RabbitMQProductUpdateConsumer> logger, IDistributedCache cache)
    {
        _configuration = configuration;
        _logger = logger;
        _cache = cache;

        string hostName = _configuration["RABBITMQ_HostName"]!;
        string port = _configuration["RABBITMQ_Port"]!;
        string userName = _configuration["RABBITMQ_UserName"]!;
        string password = _configuration["RABBITMQ_Password"]!;

        Console.WriteLine($"RabbitMQ HostName: {hostName}");
        Console.WriteLine($"RabbitMQ Port: {port}");
        Console.WriteLine($"RabbitMQ UserName: {userName}");
        Console.WriteLine($"RabbitMQ Password: {password}");

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
        string queueName = "orders.product.update.queue";
        string routingKey = "product.update";

        // Getting the exchange name from the configuration.
        string exchangeName = _configuration["RABBITMQ_Products_Exchange"]!;

        // Create or reuse the exchange.
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);

        // Create or reuse the queue.
        _channel.QueueDeclare(queueName, true, false, false, null);

        // Bind the queue to the exchange with the routing key.
        _channel.QueueBind(queueName, exchangeName, routingKey);

        EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) =>
        {
            byte[] body = args.Body.ToArray();
            string stringMessage = Encoding.UTF8.GetString(body);
            ProductUpdateMessage message = JsonSerializer.Deserialize<ProductUpdateMessage>(stringMessage)!;

            await HandleProductUpdate(message);
        };

        _channel.BasicConsume(queueName, true, consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }

    private async Task HandleProductUpdate(ProductUpdateMessage message)
    {
        _logger.LogInformation("Received product update message for ProductID: {ProductID}", message.ProductID);
        string cachedKey = $"product:{message.ProductID}";
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(5)).SetSlidingExpiration(TimeSpan.FromSeconds(3));
        string messageJSONForCache = JsonSerializer.Serialize(message);

        await _cache.SetStringAsync(cachedKey, messageJSONForCache, options);
    }
}
