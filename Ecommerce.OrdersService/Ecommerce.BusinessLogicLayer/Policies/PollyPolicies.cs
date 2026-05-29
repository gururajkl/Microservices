using Ecommerce.BusinessLogicLayer.Policies.Contracts;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace Ecommerce.BusinessLogicLayer.Policies;

internal class PollyPolicies(ILogger<PollyPolicies> logger) : IPollyPolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
    {
        // The policy will retry the request up to retryCount times with an exponential backoff strategy if the response is not successful.
        AsyncRetryPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (response, timespan, retryCount, context) =>
            {
                logger.LogInformation("Retrying request to the service. Retry attempt {RetryCount} after {TimeSpan}", retryCount, timespan);
            });

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int openCircuitAfterRetryCount, TimeSpan durationOfBreak)
    {
        // The circuit breaker policy will open the circuit after openCircuitAfterRetryCount failure requests to the service.
        AsyncCircuitBreakerPolicy<HttpResponseMessage> policy =
            Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
            .CircuitBreakerAsync(openCircuitAfterRetryCount, durationOfBreak,
            onBreak: (response, timespan) =>
            {
                logger.LogWarning("Breaking circuit for {TimeSpan} due to response: {Response}", timespan, response);
            },
            onReset: () =>
            {
                logger.LogInformation("Circuit breaker reset. Resuming normal operation.");
            });

        return policy;
    }

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout)
    {
        // The timeout policy will cancel the request if it takes longer than the specified timeout duration.
        AsyncTimeoutPolicy<HttpResponseMessage> policy = Policy.TimeoutAsync<HttpResponseMessage>(timeout, TimeoutStrategy.Optimistic,
            onTimeoutAsync: (context, timespan, task) =>
            {
                logger.LogWarning("Timeout occurred after {TimeSpan}", timespan);
                return Task.CompletedTask;
            });

        return policy;
    }
}
