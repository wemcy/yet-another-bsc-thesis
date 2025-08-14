using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService
{
    private readonly IDictionary<Guid, Recipe> recipies;

    public RecipeService()
    {
        this.recipies = new Dictionary<Guid, Recipe>();
    }

    public void SaveRecipe(Recipe recipe)
    {
        this.recipies.Add(recipe.Id, recipe);    
    }

    public IEnumerable<Recipe> GetAllRecipe()
    {
        return [.. this.recipies.Values];
    }
}
