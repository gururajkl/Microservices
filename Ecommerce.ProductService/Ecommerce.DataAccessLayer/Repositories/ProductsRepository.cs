namespace Ecommerce.DataAccessLayer.Repositories;

public class ProductsRepository(ApplicationDbContext dbContext) : IProductsRepository
{
    public async Task<Product?> AddProductAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(Guid productID)
    {
        Product? productToDelete = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productID);

        if (productToDelete is null) return false;

        dbContext.Products.Remove(productToDelete);
        int rowsDeleted = await dbContext.SaveChangesAsync();

        return rowsDeleted > 0;
    }

    public async Task<Product?> GetProductByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        return await dbContext.Products.FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<Product?>> GetProductsAsync()
    {
        return await dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByConditionAsync(Expression<Func<Product, bool>> expression)
    {
        return await dbContext.Products.Where(expression).ToListAsync();
    }

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
