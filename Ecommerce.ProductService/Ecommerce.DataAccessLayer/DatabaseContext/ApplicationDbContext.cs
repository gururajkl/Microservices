using Ecommerce.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataAccessLayer.DatabaseContext;

/// <summary>
/// Database context for the application.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Products DbSet represents the collection of the <see cref="Product"/> entities.
    /// </summary>
    public DbSet<Product> Products { get; set; }
}
