namespace Ecommerce.Core.Entities;

/// <summary>
/// Model class representing an application user details in data store.
/// </summary>
public class ApplicationUser
{
    public Guid UserID { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PersonName { get; set; }
    public string? Gender { get; set; }
}
