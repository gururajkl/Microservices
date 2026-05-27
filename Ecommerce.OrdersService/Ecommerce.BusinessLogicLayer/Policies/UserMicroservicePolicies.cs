using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Ecommerce.BusinessLogicLayer.Policies;

public class UserMicroservicePolicies(ILogger<UserMicroservicePolicies> logger) : IUserMicroservicePolicies
{
    // The policy will retry the request up to 5 times with a delay of 3 seconds between each retry if the response is not successful.
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3), onRetry: (response, timespan, retryCount, context) =>
            {
                logger.LogInformation("Retrying request to users microservice. Attempt {RetryCount}. Waiting {TimeSpan} before next retry", retryCount,
                    timespan);
            });

        return policy;
    }
}
