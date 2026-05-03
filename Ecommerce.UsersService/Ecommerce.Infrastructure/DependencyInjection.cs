using Ecommerce.Core.RepositoryContracts;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method can be used to add infrastructure services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IUsersRepository, UsersRepository>();
        return services;
    }
}
