using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace Ecommerce.BusinessLogicLayer.Services;

internal class ProductsService : IProductsService
{
    public Task<ProductResponse?> AddProductAsync(ProductAddRequest product)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProductAsync(Guid productID)
    {
        throw new NotImplementedException();
    }

    public Task<ProductResponse?> GetProductByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductResponse?>> GetProductsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductResponse?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public Task<ProductResponse?> UpdateProductAsync(ProductUpdateRequest product)
    {
        throw new NotImplementedException();
    }
}
