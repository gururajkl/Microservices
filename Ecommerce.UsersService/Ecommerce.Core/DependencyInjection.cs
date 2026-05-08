using Ecommerce.Core.ServiceContracts;
using Ecommerce.Core.Services;
using Ecommerce.Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method can be used to add core services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // Register the UsersService with the dependency injection container.
        services.AddTransient<IUsersService, UsersService>();

        // Register all validators from the assembly containing LoginRequestValidator.
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        return services;
    }
}
