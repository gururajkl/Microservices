namespace Ecommerce.Core.DTO;

/// <summary>
/// Login request model class used to capture the login request details from the user.
/// </summary>
/// <param name="Email">Email of the user.</param>
/// <param name="Password">Password of the user.</param>
public record LoginRequest(string? Email, string? Password);
