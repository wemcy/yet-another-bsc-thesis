using System.Security.Claims;

namespace Wemcy.RecipeApp.Backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor)
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
}
