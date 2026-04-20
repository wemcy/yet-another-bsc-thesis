using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Services;

public class IngredientSuggestionService(IngredientSuggestionRepository ingredientSuggestionRepository)
{

    private readonly IngredientSuggestionRepository _ingredientSuggestionRepository = ingredientSuggestionRepository;

    public IAsyncEnumerable<T> SearchIngredientsAsAsync<T>(string name)
    {
        var search = new IngredientSuggestionSearch(name);
        return _ingredientSuggestionRepository.ListIngredientsAs<T>(search);
    }

}
