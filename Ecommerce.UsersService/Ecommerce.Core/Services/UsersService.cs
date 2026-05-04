using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoryContracts;
using Ecommerce.Core.ServiceContracts;

namespace Ecommerce.Core.Services;

internal class UsersService(IUsersRepository repository) : IUsersService
{
    public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await repository.GetUserByEmailAndPasswordAsync(request.Email, request.Password);

        if (user is null) return null;

        return new(user.UserId, user.Email, user.PersonName, user.Gender, "dummyToken", true);
    }

    public async Task<AuthenticationResponse?> RegisterAsync(RegisterRequest request)
    {
        ApplicationUser userToAdd = new()
        {
            Email = request.Email,
            Password = request.Password,
            PersonName = request.PersonName,
            Gender = request.Gender?.ToString()
        };

        ApplicationUser? newUser = await repository.AddUserAsync(userToAdd);

        if (newUser is null) return null;

        return new(newUser.UserId, newUser.Email, newUser.PersonName, newUser.Gender, "dummyToken", true);
    }
}
