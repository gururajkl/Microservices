using Ecommerce.Core.DTO;
using Ecommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILogger<AuthController> logger, IUsersService service) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (request is null)
        {
            logger.LogWarning("Register request is null.");
            return BadRequest("Invalid request data.");
        }

        AuthenticationResponse? response = await service.RegisterAsync(request);

        if (response is null || !response.Success)
        {
            logger.LogWarning("Registration failed for email: {Email}.", request.Email);
            return BadRequest("Registration failed.");
        }

        logger.LogInformation("User registered successfully.");
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (request is null)
        {
            logger.LogWarning("Login request is null.");
            return BadRequest("Invalid request data.");
        }

        AuthenticationResponse? response = await service.LoginAsync(request);

        if (response is null || !response.Success)
        {
            logger.LogWarning("Login failed for email: {Email}.", request.Email);
            return Unauthorized("Login failed.");
        }

        logger.LogInformation("User logged in successfully.");
        return Ok(response);
    }
}
