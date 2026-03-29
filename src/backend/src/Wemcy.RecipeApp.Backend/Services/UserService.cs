using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IAuthorizationService authorizationService)
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

    public async Task EnsureAuthorizedAsync<T>(T resource, OperationAuthorizationRequirement operation)
    {
        var result = await authorizationService.AuthorizeAsync(GetCurrentUser(), resource, operation);
        if (!result.Succeeded)
            throw new UnauthorizedAccessException("You are not allowed to modify this resource.");
    }

    public async Task<AppUser> GetUserByIdAsync(Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString()) ?? throw new UserNotFoundException(id);
    }
}
