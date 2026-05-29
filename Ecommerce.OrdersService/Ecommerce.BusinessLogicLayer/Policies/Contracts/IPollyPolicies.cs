using Polly;

namespace Ecommerce.BusinessLogicLayer.Policies.Contracts;

public interface IPollyPolicies
{
    IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount);
    IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int openCircuitAfterRetryCount, TimeSpan durationOfBreak);
    IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout);
}
