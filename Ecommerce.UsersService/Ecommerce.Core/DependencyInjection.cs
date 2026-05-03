using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method can be used to add core services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}
