using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Services;

public class AuthService(
    UserManager<IdentityUser<Guid>> userManager,
    SignInManager<IdentityUser<Guid>> signInManager) : IAuthService
{
    public async Task<RegisterResult> RegisterAsync(string email, string password)
    {
        var user = new IdentityUser<Guid>
        {
            UserName = email,
            Email = email,
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

        return new LoginResult(user.Id, user.Email!);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
