using Microsoft.Extensions.Hosting;

namespace Ecommerce.BusinessLogicLayer.RabbitMQ.Services;

public class RabbitMQProductDeleteHostedService(IRabbitMQProductDeleteConsumer consumer) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        consumer.Consume();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        consumer.Dispose();
        return Task.CompletedTask;
    }
}
