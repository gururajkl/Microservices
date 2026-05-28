using Polly;

namespace Ecommerce.BusinessLogicLayer.Policies.Contracts;

public interface IProductsMicroservicePolicies
{
    IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy();
}
