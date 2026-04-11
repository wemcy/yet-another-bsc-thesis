using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Utils;

public class RecipeIdFilter(IList<Guid> ids) : IQueryFilter<Recipe>
{
    private readonly IList<Guid> _ids = ids;

    public IQueryable<Recipe> ApplyFilters(IQueryable<Recipe> query)
    {
        return query.Where(x => _ids.Contains(x.Id));

    }
}
