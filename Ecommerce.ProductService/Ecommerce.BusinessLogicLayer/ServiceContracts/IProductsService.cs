using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace Ecommerce.BusinessLogicLayer.ServiceContracts;

public interface IProductsService
{
    Task<List<ProductResponse?>> GetProductsAsync();
    Task<List<ProductResponse?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> expression);
    Task<ProductResponse?> GetProductByConditionAsync(Expression<Func<Product, bool>> expression);
    Task<ProductResponse?> AddProductAsync(ProductAddRequest product);
    Task<ProductResponse?> UpdateProductAsync(ProductUpdateRequest product);
    Task<bool> DeleteProductAsync(Guid productID);
}
