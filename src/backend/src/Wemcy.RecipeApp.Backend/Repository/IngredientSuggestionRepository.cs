using AutoMapper.QueryableExtensions;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Repository;

public class IngredientSuggestionRepository(DatabaseContext databaseContext, IMapper mapper)
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IMapper _mapper = mapper;

    public  IAsyncEnumerable<T> ListIngredientsAs<T>( IQueryFilter<IngredientSuggestion> filter)
    {
        return  _databaseContext.IngredientSuggestions
            .AsNoTracking()
            .WithFilter(filter)
            .OrderByDescending(x => x.UpdatedAt)
            .ProjectTo<T>(_mapper.ConfigurationProvider)
            .ToAsyncEnumerable();
    }
}
