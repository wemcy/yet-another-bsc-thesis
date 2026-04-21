using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Services;

public class IngredientSuggestionService(IIngredientSuggestionRepository ingredientSuggestionRepository, IMapper mapper) : IIngredientSuggestionService
{
    readonly IMapper _mapper = mapper;

    private readonly IIngredientSuggestionRepository _ingredientSuggestionRepository = ingredientSuggestionRepository;
    public IAsyncEnumerable<T> SearchIngredientsAsAsync<T>(string name)
    {
        var search = new IngredientSuggestionSearch(name);
        return _ingredientSuggestionRepository.ListIngredientsAs<T>(search);
    }

    public async Task<IngredientSuggestion> CreateIngredientAsync(IngredientSuggestion ingredientSuggestion)
    {
        var newIngredient = _ingredientSuggestionRepository.AddIngredient(ingredientSuggestion);
        await _ingredientSuggestionRepository.SaveAsync();
        return newIngredient;
    }

    public async Task DeleteIngredientAsync(Guid id)
    {
        var ingredient = await _ingredientSuggestionRepository.GetIngredientByIdAsync(id);
        _ingredientSuggestionRepository.DeleteIngredient(ingredient);
        await _ingredientSuggestionRepository.SaveAsync();
    }

    public async Task<IngredientSuggestion> GetIngredientByIdAsync(Guid id)
    {
        var ingredient = await _ingredientSuggestionRepository.GetIngredientByIdAsync(id);
        return ingredient;
    }

    public async Task<IngredientSuggestion> UpdateIngredientByIdAsync(Guid id, Api.Models.CreateIngredientSuggestionRequest ingredientSuggestion)
    {
        var ingredient = await _ingredientSuggestionRepository.GetIngredientByIdAsync(id);
        _mapper.Map(ingredientSuggestion, ingredient);
        await _ingredientSuggestionRepository.SaveAsync();
        return ingredient;
    }
}
