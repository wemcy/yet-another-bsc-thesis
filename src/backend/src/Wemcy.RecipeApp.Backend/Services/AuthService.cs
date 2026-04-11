using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services;

public class AuthService(
    SignInManager<User> signInManager,
    IUserService userSerivice) : IAuthService
{
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IUserService _userSerivice = userSerivice;
    public async Task RegisterAsync(string email, string password, string? displayName)
    {
       await _userSerivice.CreateUserAsync(email, password, displayName);
    }

    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var user = await _userSerivice.FindUserByEmailAsync(email) ?? throw new InvalidCredentialsException();
        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new InvalidCredentialsException();

        return new LoginResponse() { Id = user.Id, Email = user.Email!, DisplayName = user.DisplayName };
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
