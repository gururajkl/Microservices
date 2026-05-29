using DnsClient.Internal;
using Ecommerce.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ecommerce.BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger, IDistributedCache distributedCache)
{
    public async Task<UserDTO?> GetUserByUserID(Guid userID)
    {
        try
        {
            string cacheKey = $"user:{userID}";

            string? cachedUser = await distributedCache.GetStringAsync(cacheKey);

            if (cachedUser is not null)
            {
                UserDTO? userFromCache = JsonSerializer.Deserialize<UserDTO>(cachedUser);
                return userFromCache;
            }

            HttpResponseMessage response = await httpClient.GetAsync($"/api/users/{userID}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    UserDTO? userFromFallback = await response.Content.ReadFromJsonAsync<UserDTO>() ?? throw new ArgumentException("Invalid user id");
                    return userFromFallback;
                }
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

            UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>() ?? throw new ArgumentException("Invalid user id");

            string userJSON = JsonSerializer.Serialize(user);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

            await distributedCache.SetStringAsync(cacheKey, userJSON, options);

            return user;
        }
        catch (BrokenCircuitException ex)
        {
            logger.LogError(ex, "Circuit breaker is open. Returning temporary unavailable user.");
            return new(Guid.Empty, "Temporary Unavailable", "Temporary Unavailable", "Temporary Unavailable");
        }
        catch (TimeoutRejectedException ex)
        {
            logger.LogError(ex, "Timeout occurred. Returning temporary unavailable user.");
            return new(Guid.Empty, "Temporary Unavailable", "Temporary Unavailable", "Temporary Unavailable");
        }
    }
}
