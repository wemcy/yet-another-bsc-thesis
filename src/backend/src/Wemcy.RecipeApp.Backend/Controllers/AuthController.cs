using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

[InvalidCredentialsHandler]
public class AuthController(IAuthService authService) : AuthApiController
{
    public async override Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var result = await authService.RegisterAsync(registerRequest.Email, registerRequest.Password, registerRequest.DisplayName);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new Register200Response
        {
            Message = "User registered successfully",
        });
    }

    public async override Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest.Email, loginRequest.Password);
        return Ok(new Login200Response
        {
            Id = result.UserId,
            Email = result.Email,
            DisplayName = result.DisplayName,
        });
    }
    public async override Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContent();
    }
}
