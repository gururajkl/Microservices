using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Ecommerce.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IDbConnection _connection;
    public IDbConnection DbConnection => _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        string? connectionStringTemplate = configuration.GetConnectionString("PostgresConnection")!;
        string? connectionString = connectionStringTemplate.Replace("$POSTGRES_HOSTNAME", Environment.GetEnvironmentVariable("POSTGRES_HOSTNAME"))
            .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"))
            .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
            .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
            .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"));

        _connection = new NpgsqlConnection(connectionString);
    }
}
