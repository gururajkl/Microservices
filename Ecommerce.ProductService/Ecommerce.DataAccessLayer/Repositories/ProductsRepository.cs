namespace Ecommerce.DataAccessLayer.Repositories;

public class ProductsRepository(ApplicationDbContext dbContext) : IProductsRepository
{
    /// <summary>
    /// Adds a new product to the database.
    /// </summary>
    /// <param name="product">The product details to save.</param>
    /// <returns>The saved product.</returns>
    public async Task<Product?> AddProductAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Deletes a product by its product ID.
    /// </summary>
    /// <param name="productID">The ID of the product to delete.</param>
    /// <returns>True when a product was deleted otherwise false.</returns>
    public async Task<bool> DeleteProductAsync(Guid productID)
    {
        Product? productToDelete = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productID);

        if (productToDelete is null) return false;

        dbContext.Products.Remove(productToDelete);
        int rowsDeleted = await dbContext.SaveChangesAsync();

        return rowsDeleted > 0;
    }

    /// <summary>
    /// Gets the first product that matches the given condition.
    /// </summary>
    /// <param name="expression">The filter used to find the product.</param>
    /// <returns>The matching product, or null when no product is found.</returns>
    public async Task<Product?> GetProductByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        return await dbContext.Products.FirstOrDefaultAsync(expression);
    }

    /// <summary>
    /// Gets all products from the database.
    /// </summary>
    /// <returns>A list of products.</returns>
    public async Task<IEnumerable<Product?>> GetProductsAsync()
    {
        return await dbContext.Products.ToListAsync();
    }

    /// <summary>
    /// Gets all products that match the given condition.
    /// </summary>
    /// <param name="expression">The filter used to find matching products.</param>
    /// <returns>A list of matching products.</returns>
    public async Task<IEnumerable<Product?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        return await dbContext.Products.Where(expression).ToListAsync();
    }

    /// <summary>
    /// Updates an existing product with new values.
    /// </summary>
    /// <param name="product">The product values to update.</param>
    /// <returns>The updated product, or null when the product is not found.</returns>
    public async Task<Product?> UpdateProductAsync(Product product)
    {
        Product? productToUpdate = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);

        if (productToUpdate is null) return null;

        productToUpdate.ProductName = product.ProductName;
        productToUpdate.Category = product.Category;
        productToUpdate.UnitPrice = product.UnitPrice;
        productToUpdate.QuantityInStock = product.QuantityInStock;

        await dbContext.SaveChangesAsync();

        return productToUpdate;
    }
}
