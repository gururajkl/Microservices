using Ecommerce.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace Ecommerce.DataAccessLayer.RepositoryContracts;

/// <summary>
/// Defines the contract for <see cref="Product"/> data access operations.
/// </summary>
public interface IProductsRepository
{
    Task<IEnumerable<Product?>> GetProductsAsync();
    Task<IEnumerable<Product?>> GetProductsByConditionAsync(Expression<Predicate<Product>> expression);
    Task<Product?> GetProductByConditionAsync(Expression<Predicate<Product>> expression);
    Task<Product?> AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productID);
}
