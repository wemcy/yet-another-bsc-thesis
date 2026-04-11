namespace Wemcy.RecipeApp.Backend.Search;

public static class QueryableExtensions
{
    extension<T>(IQueryable<T> query)
    {
        public IQueryable<T> WithFilter(IQueryFilter<T> filter)
        {
            return filter.ApplyFilters(query);
        }
    }
}
