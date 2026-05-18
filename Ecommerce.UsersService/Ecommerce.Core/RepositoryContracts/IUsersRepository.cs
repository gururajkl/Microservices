using Ecommerce.Core.Entities;

namespace Ecommerce.Core.RepositoryContracts;

public interface IUsersRepository
{
    /// <summary>
    /// Method to add a new user to the repository.
    /// </summary>
    Task<ApplicationUser?> AddUserAsync(ApplicationUser user);

    /// <summary>
    /// Method to get a user by their email and password.
    /// </summary>
    Task<ApplicationUser?> GetUserByEmailAndPasswordAsync(string? email, string? password);

    /// <summary>
    /// Method to get a user by their user id.
    /// </summary>
    Task<ApplicationUser?> GetUserByUserID(Guid? userID);
}
