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

    public Recipe SaveRecipe(Recipe recipe)
    {
        return this.recipeRepository.SaveRecipe(recipe);
    }

    public IEnumerable<Recipe> GetAllRecipe()
    {
        return this.recipeRepository.GetAllRecipe();
    }
}
