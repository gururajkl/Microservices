using Ecommerce.Core.DTO;
using Ecommerce.Core.ServiceContracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILogger<AuthController> logger, IUsersService service, IValidator<LoginRequest> loginValidator,
    IValidator<RegisterRequest> registerValidator) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var validationResult = await registerValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        if (request is null)
        {
            logger.LogWarning("Register request is null.");
            return BadRequest("Invalid request data.");
        }

        AuthenticationResponse? response = await service.RegisterAsync(request);

        if (response is null || !response.Success)
        {
            logger.LogWarning("Registration failed for email: {Email}.", request.Email);
            return BadRequest("Registration failed, please try again.");
        }

        logger.LogInformation("User registered successfully.");
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var validationResult = await loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult);
        }

        if (request is null)
        {
            logger.LogWarning("Login request is null.");
            return BadRequest("Invalid request data.");
        }

        AuthenticationResponse? response = await service.LoginAsync(request);

        if (response is null || !response.Success)
        {
            logger.LogWarning("Login failed for email: {Email}.", request.Email);
            return Unauthorized("Login failed, please check your credentials.");
        }

        logger.LogInformation("User logged in successfully.");
        return Ok(response);
    }
}
