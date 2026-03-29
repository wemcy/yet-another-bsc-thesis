namespace Wemcy.RecipeApp.Backend.Pagination;

public static class PaginationExtensions
{

    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, PaginationOptions options)
    {
        return await PaginatedResult<T>.CreateAsync(source, options);
    }

}
