using Ecommerce.BusinessLogicLayer.DTO;
using System.Net;
using System.Net.Http.Json;

namespace Ecommerce.BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient(HttpClient httpClient)
{
    public async Task<ProductDTO?> GetProductByProductID(Guid productID)
    {
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
}
