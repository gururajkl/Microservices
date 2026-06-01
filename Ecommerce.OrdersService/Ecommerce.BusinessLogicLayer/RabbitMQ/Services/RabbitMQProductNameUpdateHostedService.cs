using Microsoft.Extensions.Hosting;

namespace Ecommerce.BusinessLogicLayer.RabbitMQ.Services;

/// <summary>
/// Hosted service that starts the RabbitMQ consumer for product name updates when the application starts and stops.
/// </summary>
/// <param name="consumer"><see cref="RabbitMQProductNameUpdateConsumer"/> which consumes the specified messages from RabbitMQ.</param>
public class RabbitMQProductNameUpdateHostedService(IRabbitMQProductNameUpdateConsumer consumer) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        consumer.Consume();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        (consumer as RabbitMQProductNameUpdateConsumer)?.Dispose();
        return Task.CompletedTask;
    }
}
