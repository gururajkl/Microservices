namespace Ecommerce.Core.DTO;

/// <summary>
/// Register request model class used to capture the registration request details from the user.
/// </summary>
/// <param name="Email">Email of the user.</param>
/// <param name="Password">Password of the user.</param>
/// <param name="PersonName">Name of the user.</param>
/// <param name="Gender">Gender of the user.</param>
public record RegisterRequest(string? Email, string? Password, string? PersonName, GenderOptions? Gender);
