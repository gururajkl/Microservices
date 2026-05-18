using Ecommerce.Core.DTO;
using Ecommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUsersService service) : ControllerBase
{
    [HttpGet("{userID:guid}")]
    public async Task<IActionResult> GetUser(Guid userID)
    {
        if (userID == Guid.Empty)
        {
            return BadRequest("User id is not valid");
        }

        UserDTO? user = await service.GetUserByUserID(userID);

        if (user == null)
        {
            return NotFound(user);
        }

        return Ok(user);
    }
}
