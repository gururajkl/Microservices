using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Fallback;
using System.Net;
using System.Text.Json;

namespace Ecommerce.BusinessLogicLayer.Policies;

internal class ProductsMicroservicePolicies(ILogger<ProductsMicroservicePolicies> logger) : IProductsMicroservicePolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy()
    {
        // The fallback policy will return a temporary unavailable product when the products microservice is unavailable.
        AsyncFallbackPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .FallbackAsync(async (context) =>
            {
                logger.LogInformation("Products microservice is unavailable. Returning fallback response.");

                ProductDTO productDTO = new(Guid.Empty, "Temporary Unavailable", "Temporary Unavailable", 0, 0);

                HttpResponseMessage responseMessage = new(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(productDTO), System.Text.Encoding.UTF8, "application/json")
                };

                return responseMessage;
            });

        return policy;
    }
}
