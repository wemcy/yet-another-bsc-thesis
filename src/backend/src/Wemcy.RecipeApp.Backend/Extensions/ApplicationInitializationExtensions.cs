using Microsoft.AspNetCore.Identity;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Extensions;

public static class ApplicationInitializationExtensions
{
    public static async Task EnsureDefaultAdminAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        await userService.CreateAdminUser();

       
        // TODO Showcased recipes creation, and featured recipe when we first start up the appplication
    }
}
