using AutoMapper;
using Ecommerce.Core.DTO;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoryContracts;
using Ecommerce.Core.ServiceContracts;

namespace Ecommerce.Core.Services;

internal class UsersService(IUsersRepository repository, IMapper mapper) : IUsersService
{
    public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
    {
        ApplicationUser? user = await repository.GetUserByEmailAndPasswordAsync(request.Email, request.Password);

        if (user is null) return null;

        return mapper.Map<AuthenticationResponse>(user) with { Success = true, Token = "dummyToken" };
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

        return mapper.Map<AuthenticationResponse>(newUser) with { Success = true, Token = "dummyToken" };
    }
}
