using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IAuthorizationService authorizationService, RoleManager<IdentityRole<Guid>> roleManager)
{
    public ClaimsPrincipal GetCurrentUser()
    {
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

    public async Task CreateAdminUser()
    {
        await EnsureRoleCreated(Roles.Admin);

        const string defaultAdminEmail = "admin@recipe-app.local";
        const string defaultAdminPassword = "Admin123!";
        const string defaultAdminDisplayName = "Administrator";

        var defaultAdmin = await CreateUserIfNotExist(defaultAdminEmail, defaultAdminPassword, defaultAdminDisplayName);

        await AddRoleToUser(defaultAdmin, Roles.Admin);
    }

    private async Task AddRoleToUser(AppUser appUser, string role)
    {
        if (!await userManager.IsInRoleAsync(appUser, role))
        {
            var roleResult = await userManager.AddToRoleAsync(appUser, role);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not assign {role} role to user {appUser.UserName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    private async Task<AppUser> CreateUserIfNotExist(string email, string password, string displayName)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null) return user;
        var newUser = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            DisplayName = displayName,
            Image = null,
        };

        var createResult = await userManager.CreateAsync(newUser, password);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Could not create user {email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }
        return newUser;
    }

    private async Task EnsureRoleCreated(string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}
