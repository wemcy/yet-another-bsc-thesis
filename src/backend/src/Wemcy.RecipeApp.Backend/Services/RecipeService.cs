using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService
{
    private readonly RecipeRepository recipeRepository;

    public RecipeService(RecipeRepository recipeRepository)
    {
        this.recipeRepository = recipeRepository;
    }

    public Recipe CreateRecipe(Recipe recipe)
    {
        var currentTime = DateTime.Now;
        recipe.CreatedAt = currentTime;
        recipe.UpdatedAt = currentTime;
        return this.recipeRepository.SaveRecipe(recipe);
    }

    public IEnumerable<Recipe> GetAllRecipe()
    {
        return this.recipeRepository.GetAllRecipe();
    }
}
