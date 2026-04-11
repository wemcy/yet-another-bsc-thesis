
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Repository;


namespace Wemcy.RecipeApp.Backend.Services;

public class ShowcaseRecipeService(RecipeService recipeService, RecipeShowcaseRepository recipeShowcaseRepository)
{
    public async Task<IList<T>> GetShowcaseRecipes<T>()
    {
        var ids = await recipeShowcaseRepository.GetShowcaseRecipeIds();
        var recipes = await recipeService.ListResipesAs<T>(new GuidFilter(ids));
        return recipes;
    }

    public async Task<Recipe> GetFeaturedRecipeAsync()
    {
        return await recipeShowcaseRepository.GetFeaturedRecipe();
    }

    public async Task SetFeaturedRecipe(Guid featuredRecipeId)
    {
        var featuredRecipe = await recipeService.GetRecipeByIdAsync(featuredRecipeId);
        var showcase = await recipeShowcaseRepository.GetRecipeShowcase();
        showcase.FeaturedRecipe = featuredRecipe;
        await recipeShowcaseRepository.SaveAsync();
    }

    public async Task UpdateShowcaseRecipes()
    {
        var recipeIds = await recipeService.GetRandomRecipesGuids(6);
        var showcase = await recipeShowcaseRepository.GetRecipeShowcase();
        showcase.ShowcaseRecipeIds = [.. recipeIds];
        await recipeShowcaseRepository.SaveAsync();
    }
}
