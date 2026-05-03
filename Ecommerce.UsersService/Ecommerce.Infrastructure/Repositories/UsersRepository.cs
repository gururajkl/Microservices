using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoryContracts;

namespace Ecommerce.Infrastructure.Repositories;

internal class UsersRepository : IUsersRepository
{
    public async Task<ApplicationUser?> AddUserAsync(ApplicationUser user)
    {
        // Mock data.
        user.UserId = Guid.NewGuid();
        return user;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPasswordAsync(string? email, string? password)
    {
        // Mock data.
        return new()
        {
            UserId = Guid.NewGuid(),
            Email = email,
            Password = password,
            PersonName = "John Doe",
            Gender = GenderOptions.Male.ToString()
        };
    }
}
