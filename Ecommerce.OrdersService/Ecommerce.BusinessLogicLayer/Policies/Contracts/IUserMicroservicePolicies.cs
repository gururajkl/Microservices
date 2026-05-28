using Polly;

namespace Ecommerce.BusinessLogicLayer.Policies.Contracts;

/// <summary>
/// Provides resilience and fault handling policies for HTTP communication with the user microservice.
/// </summary>
public interface IUserMicroservicePolicies
{
    IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
    IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
    IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy();
}
