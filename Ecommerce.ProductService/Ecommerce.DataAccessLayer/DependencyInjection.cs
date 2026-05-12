namespace Ecommerce.DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionStringFromJSON = configuration.GetConnectionString("MySQLConnectionString")!;

        // Replace with environment values.
        string connectionString = connectionStringFromJSON.Replace("$MYSQL_HOSTNAME", Environment.GetEnvironmentVariable("MYSQL_HOSTNAME"))
            .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));

        // Register ApplicationDbContext with MySQL provider and connection string from configuration.
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(connectionString);
        });

        services.AddScoped<IProductsRepository, ProductsRepository>();

        return services;
    }
}
