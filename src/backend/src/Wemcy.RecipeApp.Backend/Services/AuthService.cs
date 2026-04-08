using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services;

public class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager)
{
    public async Task<RegisterResult> RegisterAsync(string email, string password, string? displayName)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? email : displayName.Trim(),
            Image = null,
        };

        var result = await userManager.CreateAsync(user, password);
        return new RegisterResult(result.Succeeded, result.Errors);
    }

    public async Task<LoginResult> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email)
            ?? throw new InvalidCredentialsException();

        var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new InvalidCredentialsException();

        return new LoginResult(user.Id, user.Email!, user.DisplayName);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
