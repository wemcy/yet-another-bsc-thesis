using AutoMapper.QueryableExtensions;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Repository;

public class IngredientSuggestionRepository(DatabaseContext databaseContext, IMapper mapper) : IIngredientSuggestionRepository
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IMapper _mapper = mapper;

    public IAsyncEnumerable<T> ListIngredientsAs<T>(IQueryFilter<IngredientSuggestion> filter)
    {
        return _databaseContext.IngredientSuggestions
            .AsNoTracking()
            .WithFilter(filter)
            .OrderByDescending(x => x.UpdatedAt)
            .ProjectTo<T>(_mapper.ConfigurationProvider)
            .ToAsyncEnumerable();
    }

    public async Task<IngredientSuggestion> GetIngredientByIdAsync(Guid id)
    {
        return await _databaseContext.IngredientSuggestions
             .Where(x => x.Id == id)
            .SingleOrDefaultAsync() ?? throw new RecipeNotFoundException(id);
    }

    public async Task SaveAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public void DeleteIngredient(IngredientSuggestion ingredient)
    {
        _databaseContext.IngredientSuggestions.Remove(ingredient);
    }

    public IngredientSuggestion AddIngredient(IngredientSuggestion ingredientSuggestion)
    {
        _databaseContext.IngredientSuggestions.Add(ingredientSuggestion);
        return ingredientSuggestion;
    }
}
