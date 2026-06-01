using Ecommerce.BusinessLogicLayer.Mappers;
using Ecommerce.BusinessLogicLayer.Policies;
using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Ecommerce.BusinessLogicLayer.RabbitMQ;
using Ecommerce.BusinessLogicLayer.RabbitMQ.Services;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.BusinessLogicLayer.Services;
using Ecommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Fluent validation into the pipeline.
        services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

        // Add Auto mapper into the pipeline.
        services.AddAutoMapper(config => { }, typeof(OrderAddRequestToOrderMappingProfile).Assembly);

        services.AddScoped<IOrdersService, OrderService>();

        // Register policies for microservice clients.
        services.AddTransient<IUserMicroservicePolicies, UserMicroservicePolicies>();
        services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();
        services.AddTransient<IPollyPolicies, PollyPolicies>();
        services.AddTransient<IRabbitMQProductNameUpdateConsumer, RabbitMQProductNameUpdateConsumer>();

        // Add Redis cache configuration.
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}";
        });

        services.AddHostedService<RabbitMQProductNameUpdateHostedService>();

        return services;
    }
}
