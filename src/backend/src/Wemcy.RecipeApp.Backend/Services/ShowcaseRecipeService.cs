using System.Collections.Concurrent;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Utils;


namespace Wemcy.RecipeApp.Backend.Services;

public class ShowcaseRecipeService(IRecipeService recipeService, IRecipeShowcaseRepository recipeShowcaseRepository, IRecipeCache recipeCache) : IShowcaseRecipeService
{

    public async Task<IList<Api.Models.Recipe>> GetShowcaseRecipesAsync()
    {
        var ids = await recipeShowcaseRepository.GetShowcaseRecipeIds();
        if (recipeCache.GetRecipesFromCache(ids, out var cachedRecipes))
        {
            return cachedRecipes;
        }
        var recipes = await recipeService.ListResipesAsAsync<Api.Models.Recipe>(new RecipeIdFilter(ids));
        foreach (var recipe in recipes)
        {
            recipeCache.StoreCache(recipe);
        }
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
