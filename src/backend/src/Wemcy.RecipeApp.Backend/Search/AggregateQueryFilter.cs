namespace Wemcy.RecipeApp.Backend.Search;

public class AggregateQueryFilter<T> : IQueryFilter<T>
{
    private readonly IList<IQueryFilter<T>> filters;

    public IReadOnlyList<IQueryFilter<T>> Filters => [.. filters];
    public AggregateQueryFilter()
    {
        filters = [];
    }
    public IQueryable<T> ApplyFilters(IQueryable<T> query)
    {
        foreach (var filter in filters)
        {
            query = filter.ApplyFilters(query);
        }
        return query;
    }

    public void AddFilter(IQueryFilter<T> filter)
    {
        filters.Add(filter);
    }

}
