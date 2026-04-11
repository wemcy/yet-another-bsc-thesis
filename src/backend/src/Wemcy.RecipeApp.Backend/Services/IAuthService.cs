using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task RegisterAsync(string email, string password, string? displayName);
    }
}
