using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService(RecipeRepository recipeRepository)
{
    private readonly RecipeRepository recipeRepository = recipeRepository;

    public Recipe CreateRecipe(Recipe recipe)
    {
        var currentTime = DateTimeOffset.UtcNow;
        recipe.CreatedAt = currentTime;
        recipe.UpdatedAt = currentTime;
        return this.recipeRepository.SaveRecipe(recipe);
    }

    public IQueryable<Recipe> GetAllRecipe()
    {
        return this.recipeRepository.GetAllRecipe();
    }

    public Recipe? GetRecipeById(Guid id)
    {
        return this.recipeRepository.GetRecipeById(id);
    }
}
