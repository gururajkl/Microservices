using AutoMapper;
using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.RabbitMQ;
using Ecommerce.BusinessLogicLayer.RabbitMQ.MessageTypes;
using Ecommerce.BusinessLogicLayer.ServiceContracts;
using Ecommerce.DataAccessLayer.Entities;
using Ecommerce.DataAccessLayer.RepositoryContracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Ecommerce.BusinessLogicLayer.Services;

internal class ProductsService(IProductsRepository repository, IMapper mapper,
    IValidator<ProductAddRequest> addRequestValidator, IValidator<ProductUpdateRequest> updateRequestValidator,
    IRabbitMQPublisher rabbitMqPublisher, ILogger<ProductsService> logger) : IProductsService
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

    public async Task<bool> DeleteProductAsync(Guid productID)
    {
        Product? productInDb = await repository.GetProductByConditionAsync(p => p.ProductID == productID);

        if (productInDb is null) return false;

        bool result = await repository.DeleteProductAsync(productID);

        if (result)
        {
            // Publish a message to RabbitMQ about the product deletion.
            string rountingKey = "product.delete";
            ProductDeleteMessage message = new(productInDb.ProductID, productInDb.ProductName);
            rabbitMqPublisher.Publish(rountingKey, message);
            logger.LogInformation("Published product deletion message for ProductID: {ProductID}", productID);
        }

        return result;
    }

    public async Task<ProductResponse?> GetProductByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        Product? product = await repository.GetProductByConditionAsync(expression);

        if (product is null) return null;

        return mapper.Map<ProductResponse>(product);
    }

    public async Task<List<ProductResponse?>> GetProductsAsync()
    {
        IEnumerable<Product?> products = await repository.GetProductsAsync();

        IEnumerable<ProductResponse> productResponses = mapper.Map<IEnumerable<ProductResponse>>(products);

        return [.. productResponses];
    }

    public async Task<List<ProductResponse?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        IEnumerable<Product?> products = await repository.GetProductsByConditionAsync(expression);

        IEnumerable<ProductResponse> productResponses = mapper.Map<IEnumerable<ProductResponse>>(products);

        return [.. productResponses];
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

        Product? existingProduct = await repository.GetProductByConditionAsync(p => p.ProductID == productUpdateRequest.ProductID)
            ?? throw new ArgumentException("Invalid product ID");

        Product productToUpdate = mapper.Map<Product>(productUpdateRequest);

        Product? updatedProduct = await repository.UpdateProductAsync(productToUpdate);

        // Publish a message to RabbitMQ about the product update.
        string routingKey = "product.update";
        ProductUpdateMessage message = new(productUpdateRequest.ProductID, productUpdateRequest.ProductName,
            productToUpdate.Category, productUpdateRequest.UnitPrice, productToUpdate.QuantityInStock);

        rabbitMqPublisher.Publish<ProductUpdateMessage>(routingKey, message);

        if (updatedProduct is null) return null;

        return mapper.Map<ProductResponse>(updatedProduct);
    }
}
