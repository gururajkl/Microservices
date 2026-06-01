using Ecommerce.BusinessLogicLayer.Mappers;
using Ecommerce.BusinessLogicLayer.RabbitMQ;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.BusinessLogicLayer.Services;
using Ecommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();
        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddAutoMapper(config => { }, typeof(ProductToProductResponseMappingProfile).Assembly);

        // Register RabbitMQPublisher as a transient service.
        services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();

        return services;
    }
}
