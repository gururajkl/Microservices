namespace Ecommerce.Core.DTO;

/// <summary>
/// Record class representing the authentication response details returned to the user after successful or failed login or registration.
/// </summary>
/// <param name="UserID">User id.</param>
/// <param name="Email">Email of the user.</param>
/// <param name="PersonName">Name of the user.</param>
/// <param name="Gender">Gender of the user.</param>
/// <param name="Token">Authentication token.</param>
/// <param name="Success">Indicates whether the authentication was successful.</param>
public record AuthenticationResponse(Guid UserID, string? Email, string? PersonName, string? Gender, string? Token, bool Success)
{
    // Parameterless constructor for serialization purposes for auto mapper.
    public AuthenticationResponse() : this(default, default, default, default, default, default) { }
}
