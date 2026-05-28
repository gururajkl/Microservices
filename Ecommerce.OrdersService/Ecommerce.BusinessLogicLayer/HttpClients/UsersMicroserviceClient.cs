using DnsClient.Internal;
using Ecommerce.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using System.Net;
using System.Net.Http.Json;

namespace Ecommerce.BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger)
{
    public async Task<UserDTO?> GetUserByUserID(Guid userID)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/api/users/{userID}");

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
                    // For other status codes, we can choose to throw an exception or return null. Here, we will return a temporary error user as Fault data.
                    return new(Guid.Empty, "Temporary Unavailable", "Temporary Unavailable", "Temporary Unavailable");
                }
            }

            UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();

            return user is null ? throw new ArgumentException("Invalid user id") : user;
        }
        catch (BrokenCircuitException ex)
        {
            logger.LogError(ex, "Circuit breaker is open. Returning temporary unavailable user.");
            return new(Guid.Empty, "Temporary Unavailable", "Temporary Unavailable", "Temporary Unavailable");
        }
    }
}
