using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
    {
        return services;
    }
}
