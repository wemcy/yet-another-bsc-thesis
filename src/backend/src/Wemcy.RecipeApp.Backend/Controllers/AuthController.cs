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
[UserNotFoundHandler]
public class AuthController(IAuthService authService) : AuthApiController
{
    public async override Task<IActionResult> Register([FromBody] Api.Models.RegisterRequest registerRequest)
    {
        var result = await authService.RegisterAsync(registerRequest.Email, registerRequest.Password, registerRequest.DisplayName);
        if (!result.Succeeded)
            return BadRequest(result.Errors);
        var user = await authService.LoginAsync(registerRequest.Email, registerRequest.Password);
        return Ok(new LoginResponse
        {
            Id = user.UserId,
            Email = user.Email,
            DisplayName = user.DisplayName,
        });
    }

    public async override Task<IActionResult> Login([FromBody] Api.Models.LoginRequest loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest.Email, loginRequest.Password);
        return Ok(new LoginResponse
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

    [HttpPost("/auth/admin/promote")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> PromoteToAdmin([FromBody] PromoteToAdminRequest request)
    {
        await authService.MakeUserAdminAsync(request.Email);
        return NoContent();
    }
}

public class PromoteToAdminRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
