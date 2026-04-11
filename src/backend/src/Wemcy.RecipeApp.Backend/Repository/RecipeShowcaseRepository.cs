using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Repository;

public class RecipeShowcaseRepository(DatabaseContext databaseContext)
{
    public async Task<Recipe> GetFeaturedRecipe()
    {
        return (await GetRecipeShowcase())?.FeaturedRecipe ?? throw new RecipeNotFoundException(Guid.Empty);
    }

    public async Task<IList<Guid>> GetShowcaseRecipeIds()
    {
        var showcase = await GetRecipeShowcase();
        return showcase?.ShowcaseRecipeIds.ToList() ?? [];
    }

    public async Task SaveAsync()
    {
        await databaseContext.SaveChangesAsync();
    }

    public async Task<RecipeShowcase> GetRecipeShowcase()
    {
        return await databaseContext.FindAsync<RecipeShowcase>(RecipeShowcase.SingletonId) ?? throw new InvalidOperationException("Recipe showcase not found.");
    }
}
