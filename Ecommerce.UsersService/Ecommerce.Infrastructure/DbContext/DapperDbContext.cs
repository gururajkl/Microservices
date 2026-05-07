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
        string? connectionString = configuration.GetConnectionString("PostgresConnection");
        _connection = new NpgsqlConnection(connectionString);
    }
}
