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
        app.MapGet("/api/products/search/product-id/{productID}", async (IProductsService productsService, Guid productID) =>
        {
            ProductResponse? product = await productsService.GetProductByConditionAsync(p => p.ProductID == productID);
            return Results.Ok(product);
        });

        return app;
    }
}
