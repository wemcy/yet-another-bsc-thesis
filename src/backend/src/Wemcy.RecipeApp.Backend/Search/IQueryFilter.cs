namespace Wemcy.RecipeApp.Backend.Search;

public interface IQueryFilter<T>
{
    IQueryable<T> ApplyFilters(IQueryable<T> query);
}
