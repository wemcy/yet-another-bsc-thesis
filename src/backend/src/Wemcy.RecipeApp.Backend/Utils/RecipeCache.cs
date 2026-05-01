namespace Wemcy.RecipeApp.Backend.Utils;

public class RecipeCache()
{
    private Cache<Guid, Api.Models.Recipe> _cache = new(50);
    public void InvalidateCache()
    {
        _cache.Clear();
    }
    public void StoreCache( Api.Models.Recipe recipe) 
    {
        _cache.Add(recipe.Id, recipe);
    }

    public Api.Models.Recipe? GetRecipeFromCache(Guid recipeId) 
    { 
        _cache.TryGet(recipeId, out var recipe);
        return recipe;
    }

    public bool GetRecipesFromCache(IEnumerable<Guid> recipeIds,out IList<Api.Models.Recipe> recipes)
    {
        recipes = [];
        foreach (var recipeId in recipeIds)
        {
            if (_cache.TryGet(recipeId, out var recipe))
            {
                recipes.Add(recipe);
            } else {
                return false;
            }
        }
        return true;
    }
}
