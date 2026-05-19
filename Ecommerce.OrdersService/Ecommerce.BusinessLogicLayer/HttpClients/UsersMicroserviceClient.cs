using Ecommerce.BusinessLogicLayer.DTO;
using System.Net;
using System.Net.Http.Json;

namespace Ecommerce.BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient(HttpClient httpClient)
{
    public async Task<UserDTO?> GetUserByUserID(Guid userID)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"api/users/{userID}");

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new HttpRequestException("Bad request", null, HttpStatusCode.BadRequest);
            }
        }
        else
        {
            throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
        }

        UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO>();

        return user is null ? throw new ArgumentException("Invalid user id") : user;
    }
}
