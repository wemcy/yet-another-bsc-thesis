using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Services;

public interface IIngredientSuggestionService
{
    Task<Model.Entities.IngredientSuggestion> CreateIngredientAsync(Model.Entities.IngredientSuggestion ingredientSuggestion);
    Task DeleteIngredientAsync(Guid id);
    Task<Model.Entities.IngredientSuggestion> GetIngredientByIdAsync(Guid id);
    IAsyncEnumerable<T> SearchIngredientsAsAsync<T>(string name);
    Task<Model.Entities.IngredientSuggestion> UpdateIngredientByIdAsync(Guid id, CreateIngredientSuggestionRequest ingredientSuggestion);
}
