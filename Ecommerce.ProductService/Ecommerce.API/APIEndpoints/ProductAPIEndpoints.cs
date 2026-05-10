namespace Ecommerce.API.APIEndpoints;

public static class ProductAPIEndpoints
{
    /// <summary>
    /// Maps all <see cref="Product"/> API endpoints to the <see cref="WebApplication"/>.
    /// </summary>
    public static IEndpointRouteBuilder MapProductAPIEndpoints(this IEndpointRouteBuilder app)
    {
        // GET, /api/products.
        app.MapGet("/api/products", async (IProductsService productsService) =>
        {
            List<ProductResponse?> products = await productsService.GetProductsAsync();
            return Results.Ok(products);
        });

        // GET, /api/products/search/product-id/ID.
        app.MapGet("/api/products/search/product-id/{productID:guid}", async (IProductsService productsService, Guid productID) =>
        {
            ProductResponse? product = await productsService.GetProductByConditionAsync(p => p.ProductID == productID);

            if (product is null) return Results.NotFound("Product is not found");

            return Results.Ok(product);
        });

        // GET, /api/products/search/searchString.
        app.MapGet("/api/products/search/{searchString}", async (IProductsService productsService, string searchString) =>
        {
            string lowerValuedSearchString = searchString.ToLower();

            List<ProductResponse?> productsByName = await productsService.GetProductsByConditionAsync(p => p.ProductName != null
            && p.ProductName.ToLower().Contains(lowerValuedSearchString));

            List<ProductResponse?> productsByCategory = await productsService.GetProductsByConditionAsync(p => p.Category != null
            && p.Category.ToLower().Contains(lowerValuedSearchString));

            var products = productsByName.Union(productsByCategory);
            return Results.Ok(products);
        });

        // POST, /api/products.
        app.MapPost("/api/products", async (IProductsService productsService, ProductAddRequest request, IValidator<ProductAddRequest> validator) =>
        {
            ValidationResult result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                Dictionary<string, string[]> errors = result.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(d => d.Key, d => d.Select(s => s.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            ProductResponse? addedProduct = await productsService.AddProductAsync(request);

            if (addedProduct is not null)
            {
                return Results.Created($"/api/products/search/product-id/{addedProduct.ProductID}", addedProduct);
            }

            return Results.Problem("Error adding the product");
        });

        // PUT, /api/products.
        app.MapPut("/api/products", async (IProductsService productsService, ProductUpdateRequest request, IValidator<ProductUpdateRequest> validator) =>
        {
            ValidationResult result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                Dictionary<string, string[]> errors = result.Errors.GroupBy(g => g.PropertyName)
                .ToDictionary(d => d.Key, d => d.Select(s => s.ErrorMessage).ToArray());

                return Results.ValidationProblem(errors);
            }

            ProductResponse? updatedProduct = await productsService.UpdateProductAsync(request);

            if (updatedProduct is not null)
            {
                return Results.Ok(updatedProduct);
            }

            return Results.Problem("Error updating the product");
        });

        // DELETE, /api/product/productID.
        app.MapDelete("/api/product/{productID:guid}", async (IProductsService productsService, Guid productID) =>
        {
            bool result = await productsService.DeleteProductAsync(productID);

            if (result) return Results.Ok(true);
            return Results.Problem("Error deleting the product");
        });

        return app;
    }
}
