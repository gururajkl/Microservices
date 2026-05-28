using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Ecommerce.BusinessLogicLayer.Policies;

public class UserMicroservicePolicies(ILogger<UserMicroservicePolicies> logger) : IUserMicroservicePolicies
{
    // The policy will retry the request up to 5 times with a delay of 2 seconds between each retry if the response is not successful.
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            // Math.Pow(2, retryAttempt) will create an exponential backoff strategy.
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), onRetry: (response, timespan, retryCount, context) =>
            {
                logger.LogInformation("Retrying request to users microservice. Attempt {RetryCount}. Waiting {TimeSpan} before next retry", retryCount,
                    timespan);
            });

        return policy;
    }

    // This policy will open the circuit after 3 failure requests to the service.
    // If circuit is open request cannot be sent.
    // If circuit is closed request can be sent.
    // If circuit is half open then only 1 request can be sent.
    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        AsyncCircuitBreakerPolicy<HttpResponseMessage> policy =
            Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromMinutes(1),
            onBreak: (response, timespan) =>
            {
                logger.LogWarning("Circuit breaker triggered for users microservice. Breaking for {TimeSpan} due to response: {Response}", timespan, response);
            },
            onReset: () =>
            {
                logger.LogInformation("Circuit breaker reset for users microservice. Resuming normal operation.");
            });

        return policy;
    }
}
