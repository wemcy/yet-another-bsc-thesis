using Microsoft.AspNetCore.Identity;

namespace Wemcy.RecipeApp.Backend.Services;

public record RegisterResult(bool Succeeded, IEnumerable<IdentityError> Errors);
public record LoginResult(Guid UserId, string Email);

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(string email, string password);
    /// <exception cref="Wemcy.RecipeApp.Backend.Exceptions.InvalidCredentialsException">Thrown when the email or password is invalid.</exception>
    Task<LoginResult> LoginAsync(string email, string password);
    Task LogoutAsync();
}
