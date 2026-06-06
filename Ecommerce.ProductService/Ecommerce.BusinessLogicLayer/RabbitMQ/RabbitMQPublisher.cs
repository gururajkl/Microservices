using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Ecommerce.BusinessLogicLayer.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;

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

    public void Publish<T>(string routingKey, T message)
    {
        // Getting the exchange name from the configuration.
        string exchangeName = _configuration["RABBITMQ_Products_Exchange"]!;

        // RabbitMQ cannot send complex objects, so we need to serialize the message to JSON and then convert it to bytes.
        string messageInJSON = JsonSerializer.Serialize(message);
        byte[] messageInBytes = Encoding.UTF8.GetBytes(messageInJSON);

        // Create the exchange if it doesn't exist. This is idempotent, so it won't create a new exchange if it already exists.
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: true);

        // Publish the message to the exchange with the specified routing key.
        _channel.BasicPublish(exchangeName, routingKey, null, messageInBytes);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
