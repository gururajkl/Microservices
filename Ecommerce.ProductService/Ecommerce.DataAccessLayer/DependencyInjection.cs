namespace Ecommerce.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Register ApplicationDbContext with MySQL provider and connection string from configuration
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("MySQLConnectionString")!);
        });

        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}
