using Dapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoryContracts;
using Ecommerce.Infrastructure.DbContext;

namespace Ecommerce.Infrastructure.Repositories;

internal class UsersRepository(DapperDbContext dbContext) : IUsersRepository
{
    public async Task<ApplicationUser?> AddUserAsync(ApplicationUser user)
    {
        user.UserID = Guid.NewGuid();

        string query = "INSERT INTO public.\"Users\" (\"UserID\", \"Email\", \"PersonName\", \"Gender\", \"Password\") VALUES(@UserID, @Email, @PersonName, @Gender, @Password)";
        int count = await dbContext.DbConnection.ExecuteAsync(query, user);

        if (count > 0)
        {
            return user;
        }

        return null;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPasswordAsync(string? email, string? password)
    {
        string query = "SELECT * FROM public.\"Users\" WHERE \"Email\" = @Email and \"Password\" = @Password";
        var parameters = new { Email = email, Password = password };

        ApplicationUser? user = await dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

        return user;
    }

    public async Task<ApplicationUser?> GetUserByUserID(Guid? userID)
    {
        string query = "SELECT * FROM public.\"Users\" WHERE \"UserID\" = @UserID";
        var parameters = new { UserID = userID };

        ApplicationUser? user = await dbContext.DbConnection.QueryFirstOrDefaultAsync(query, parameters);

        return user;
    }
}
