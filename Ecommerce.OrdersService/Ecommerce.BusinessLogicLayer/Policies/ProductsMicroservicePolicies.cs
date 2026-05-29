using Ecommerce.BusinessLogicLayer.DTO;
using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
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

                HttpResponseMessage responseMessage = new(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent(JsonSerializer.Serialize(productDTO), System.Text.Encoding.UTF8, "application/json")
                };

                return responseMessage;
            });

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy()
    {
        // The bulkhead isolation policy will allow a maximum of 2 concurrent requests to the products microservice and queue up to 40 additional requests.
        // If the queue is full, it will reject the request and log a warning.
        AsyncBulkheadPolicy<HttpResponseMessage> policy = Policy.BulkheadAsync<HttpResponseMessage>(maxParallelization: 2, maxQueuingActions: 40,
            onBulkheadRejectedAsync: (context) =>
        {
            logger.LogWarning("Bulkhead isolation triggered. Request rejected due to max parallelization or max queuing actions reached");
            throw new BulkheadRejectedException("Bulkhead queue is full");
        });

        return policy;
    }
}
