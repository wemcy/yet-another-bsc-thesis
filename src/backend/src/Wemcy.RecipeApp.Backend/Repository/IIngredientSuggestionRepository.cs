using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Repository
{
    public interface IIngredientSuggestionRepository
    {
        IngredientSuggestion AddIngredient(IngredientSuggestion ingredientSuggestion);
        void DeleteIngredient(IngredientSuggestion ingredient);
        Task<IngredientSuggestion> GetIngredientByIdAsync(Guid id);
        IAsyncEnumerable<T> ListIngredientsAs<T>(IQueryFilter<IngredientSuggestion> filter);
        Task SaveAsync();
    }
}