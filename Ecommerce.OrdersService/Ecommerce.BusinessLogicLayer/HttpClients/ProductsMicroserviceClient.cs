using Ecommerce.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger,
    IDistributedCache distributedCache)
{
    public async Task<ProductDTO?> GetProductByProductID(Guid productID)
    {
        try
        {
            string cachedKey = $"product:{productID}";

            string? cachedProduct = await distributedCache.GetStringAsync(cachedKey);

            if (cachedProduct is not null)
            {
                ProductDTO? productFromCache = JsonSerializer.Deserialize<ProductDTO>(cachedProduct);
                return productFromCache;
            }

            HttpResponseMessage response = await httpClient.GetAsync($"/api/products/search/product-id/{productID}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest);
                }
                else
                {
                    throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
                }
            }

            ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO>();

            return product is null ? throw new ArgumentException("Invalid product id") : product;
        }
        catch (BulkheadRejectedException ex)
        {
            logger.LogError(ex, "Bulkhead isolation triggered. Returning temporary unavailable product.");
            return new(Guid.Empty, "Temporary Unavailable", "Temporary Unavailable", 0, 0);
        }
    }
}
