using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

[InvalidCredentialsHandler]
[EntityNotFoundHandler]
[UnauthorizedHandler]
[RegistrationErrorHandler]

public class AuthController(AuthService authService) : AuthApiController
{
    public async override Task<IActionResult> Register([FromBody] Api.Models.RegisterRequest registerRequest)
    {
        await authService.RegisterAsync(registerRequest.Email, registerRequest.Password, registerRequest.DisplayName);
        var user = await authService.LoginAsync(registerRequest.Email, registerRequest.Password);
        return Ok(user);
    }

    public async override Task<IActionResult> Login([FromBody] Api.Models.LoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest.Email, loginRequest.Password);
        return Ok(result);
    }
    public async override Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContent();
    }
}
