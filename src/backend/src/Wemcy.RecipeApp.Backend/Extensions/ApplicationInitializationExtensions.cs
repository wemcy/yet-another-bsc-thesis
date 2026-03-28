using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Extensions;

public static class ApplicationInitializationExtensions
{
    public static async Task EnsureDefaultAdminAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Admin));
        }

        const string defaultAdminEmail = "admin@recipe-app.local";
        const string defaultAdminPassword = "Admin123!";
        const string defaultAdminDisplayName = "Administrator";

        var defaultAdmin = await userManager.FindByEmailAsync(defaultAdminEmail);
        if (defaultAdmin is null)
        {
            defaultAdmin = new AppUser
            {
                UserName = defaultAdminEmail,
                Email = defaultAdminEmail,
                EmailConfirmed = true,
                DisplayName = defaultAdminDisplayName,
                Image = null,
            };

            var createResult = await userManager.CreateAsync(defaultAdmin, defaultAdminPassword);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not create default admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(defaultAdmin, Roles.Admin))
        {
            var roleResult = await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Could not assign Admin role to default admin user: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}
