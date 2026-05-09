using AutoMapper;
using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.DataAccessLayer.Entities;
using Ecommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace Ecommerce.BusinessLogicLayer.Services;

internal class ProductsService(IProductsRepository repository, IMapper mapper,
    IValidator<ProductAddRequest> addRequestValidator, IValidator<ProductUpdateRequest> updateRequestValidator) : IProductsService
{
    public async Task<ProductResponse?> AddProductAsync(ProductAddRequest productAddRequest)
    {
        ArgumentNullException.ThrowIfNull(productAddRequest);

        ValidationResult validationResult = await addRequestValidator.ValidateAsync(productAddRequest);

        if (!validationResult.IsValid)
        {
            string errorMessages = string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new Exception(errorMessages);
        }

        Product productToAdd = mapper.Map<Product>(productAddRequest);

        Product? productAdded = await repository.AddProductAsync(productToAdd);

        if (productAdded is null) return null;

        return mapper.Map<ProductResponse>(productAdded);
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

    public async Task<ProductResponse?> UpdateProductAsync(ProductUpdateRequest productUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(productUpdateRequest);

        ValidationResult validationResult = await updateRequestValidator.ValidateAsync(productUpdateRequest);

        if (!validationResult.IsValid)
        {
            string errorMessages = string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new Exception(errorMessages);
        }

        Product productToUpdate = mapper.Map<Product>(productUpdateRequest);

        Product? updatedProduct = await repository.UpdateProductAsync(productToUpdate);

        if (updatedProduct is null) return null;

        return mapper.Map<ProductResponse>(updatedProduct);
    }
}
