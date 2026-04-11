using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Services;

public class GuidFilter : IQueryFilter<Recipe>
{
    private readonly IList<Guid> _ids;
    public GuidFilter(IList<Guid> ids)
    {
        _ids = ids;
    }
    public IQueryable<Recipe> ApplyFilters(IQueryable<Recipe> query)
    {
        return query.Where(x => _ids.Contains(x.Id));

    }
}
