using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Services;

public class ShowcaseRecipeService(DatabaseContext databaseContext, RecipeService recipeService)
{
    public async Task<IList<Recipe>> GetShowcaseRecipes()
    {
        var ids = databaseContext.RecipeShowcase.ShowcaseRecipeIds;
        return await recipeService.GetRecipesByIdsAsync(ids);
    }

    public Recipe? GetFeaturedRecipeAsync()
    {
        return databaseContext.RecipeShowcase.FeaturedRecipe ?? throw new RecipeNotFoundException(Guid.Empty);
    }

    public async Task SetFeaturedRecipe(Guid featuredRecipeId)
    {
        var featuredRecipe = await recipeService.GetRecipeByIdAsync(featuredRecipeId);
        databaseContext.RecipeShowcase.FeaturedRecipe = featuredRecipe;
        await databaseContext.SaveChangesAsync();
    }
    [Authorize(Roles = Roles.Admin)]
    public async Task UpdateShowcaseRecipes()
    {
        var recipeIds = await recipeService.GetRandomRecipesGuids(6);
        databaseContext.RecipeShowcase.ShowcaseRecipeIds = [.. recipeIds];
        await databaseContext.SaveChangesAsync();
    }
}
