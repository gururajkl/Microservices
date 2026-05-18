using Ecommerce.Core.DTO;

namespace Ecommerce.Core.ServiceContracts;

/// <summary>
/// User service contract for handling user authentication and registration.
/// </summary>
public interface IUsersService
{
    /// <summary>
    /// Method to authenticate a user based on the provided login request.
    /// </summary>
    Task<AuthenticationResponse?> LoginAsync(LoginRequest request);

    /// <summary>
    /// Method to register a new user based on the provided registration request.
    /// </summary>
    Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Method to get the user info using user id.
    /// </summary>
    Task<UserDTO?> GetUserByUserID(Guid userID);
}
