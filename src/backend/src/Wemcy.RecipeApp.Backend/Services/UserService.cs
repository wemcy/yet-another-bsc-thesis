using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
{
    public ClaimsPrincipal GetCurrentUser() {
        return httpContextAccessor?.HttpContext?.User ?? throw new UnauthorizedAccessException();
    }

    public Guid GetCurrentUserId()
    {
        var userIdValue = GetCurrentUser().FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue is null || !Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException("Authenticated user id is missing or invalid.");

        return userId;
    }

    public async Task<AppUser> GetCurrentUserEntityAsync()
    {
        var user = await userManager.FindByIdAsync(GetCurrentUserId().ToString());
        return user ?? throw new UnauthorizedAccessException("Authenticated user no longer exists.");
    }
}
