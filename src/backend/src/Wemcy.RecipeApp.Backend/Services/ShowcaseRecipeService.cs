using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Utils;


namespace Wemcy.RecipeApp.Backend.Services;

public class ShowcaseRecipeService(RecipeService recipeService, IRecipeShowcaseRepository recipeShowcaseRepository) : IShowcaseRecipeService
{
    public async Task<IList<T>> GetShowcaseRecipesAsync<T>()
    {
        var ids = await recipeShowcaseRepository.GetShowcaseRecipeIds();
        var recipes = await recipeService.ListResipesAsAsync<T>(new RecipeIdFilter(ids));
        return recipes;
    }

    public async Task<Recipe> GetFeaturedRecipeAsync()
    {
        return await recipeShowcaseRepository.GetFeaturedRecipe();
    }

    public async Task SetFeaturedRecipeAsync(Guid featuredRecipeId)
    {
        var featuredRecipe = await recipeService.GetRecipeByIdAsync(featuredRecipeId);
        var showcase = await recipeShowcaseRepository.GetRecipeShowcase();
        showcase.FeaturedRecipe = featuredRecipe;
        await recipeShowcaseRepository.SaveAsync();
    }

    public async Task UpdateShowcaseRecipesAsync()
    {
        var recipeIds = await recipeService.GetRandomRecipesGuids(6);
        var showcase = await recipeShowcaseRepository.GetRecipeShowcase();
        showcase.ShowcaseRecipeIds = [.. recipeIds];
        await recipeShowcaseRepository.SaveAsync();
    }

    public async Task CreateDeafaultShowcaseAndFeaturedRecipeAsync()
    {
        await UpdateShowcaseRecipesAsync();
        await SelectRandomFeatured();
    }

    private async Task SelectRandomFeatured()
    {
        var recipeId = (await recipeService.GetRandomRecipesGuids(1)).Single();
        await SetFeaturedRecipeAsync(recipeId);
    }
}
