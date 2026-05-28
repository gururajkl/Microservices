using Ecommerce.BusinessLogicLayer.Mappers;
using Ecommerce.BusinessLogicLayer.Policies;
using Ecommerce.BusinessLogicLayer.Policies.Contracts;
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
        // Add Fluent validation into the pipeline.
        services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();

        // Add Auto mapper into the pipeline.
        services.AddAutoMapper(config => { }, typeof(OrderAddRequestToOrderMappingProfile).Assembly);

        services.AddScoped<IOrdersService, OrderService>();

        // Register policies for microservice clients.
        services.AddTransient<IUserMicroservicePolicies, UserMicroservicePolicies>();
        services.AddTransient<IProductsMicroservicePolicies, ProductsMicroservicePolicies>();

        return services;
    }
}
