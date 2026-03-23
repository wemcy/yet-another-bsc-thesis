using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

[InvalidCredentialsHandler]
public class AuthController(IAuthService authService) : AuthApiController
{
    public async override Task<IActionResult> Register([FromBody] Api.Models.RegisterRequest registerRequest)
    {
        var result = await authService.RegisterAsync(registerRequest.Email, registerRequest.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "User registered successfully" });
    }

    public async override Task<IActionResult> Login([FromBody] Api.Models.LoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest.Email, loginRequest.Password);
        return Ok(new { id = result.UserId, email = result.Email });
    }
    public async override Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContent();
    }
}
