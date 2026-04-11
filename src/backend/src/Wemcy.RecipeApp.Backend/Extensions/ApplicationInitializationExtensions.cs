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
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        await userService.CreateAdminUser();
    }

    public static async Task EnsureDefaultShowcasesCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var showcaseRecipeService = scope.ServiceProvider.GetRequiredService<ShowcaseRecipeService>();
        await showcaseRecipeService.CreateDeafaultShowcaseAndFeaturedRecipe();
    }

    public static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<Database.DatabaseContext>();
        await databaseContext.Database.MigrateAsync();
    }
}
