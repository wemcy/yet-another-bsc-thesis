using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Repository;

public class RecipeShowcaseRepository(DatabaseContext databaseContext)
{
    public async Task<Recipe> GetFeaturedRecipe()
    {
        return (await databaseContext.FindAsync<RecipeShowcase>(RecipeShowcase.SingletonId))?.FeaturedRecipe ?? throw new RecipeNotFoundException(Guid.Empty);
    }

    public async Task<IList<Guid>> GetShowcaseRecipeIds()
    {
        var showcase = await databaseContext.FindAsync<RecipeShowcase>(RecipeShowcase.SingletonId);
        return showcase?.ShowcaseRecipeIds.ToList() ?? [];
    }

    public async Task SaveAsync()
    {
        await databaseContext.SaveChangesAsync();
    }

    public void SetFeaturedRecipe(Recipe featuredRecipe)
    {
        databaseContext.RecipeShowcase.FeaturedRecipe = featuredRecipe;
    }

    internal void SetShowcaseRecipes(IList<Guid> recipeIds)
    {
        databaseContext.RecipeShowcase.ShowcaseRecipeIds = [.. recipeIds];
    }
}
