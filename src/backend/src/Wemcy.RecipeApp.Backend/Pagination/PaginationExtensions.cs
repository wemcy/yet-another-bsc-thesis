using Microsoft.AspNetCore.Mvc;

namespace Wemcy.RecipeApp.Backend.Pagination;

public static class PaginationExtensions
{

    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, PaginationOptions options)
    {
        return await PaginatedResult<T>.CreateAsync(source, options);
    }
    public static void AddPaginationFilter(this MvcOptions mvcOptions)
    {
        mvcOptions.Filters.AddService(typeof(PaginationRequestFilter));
    }

    public static IServiceCollection ConfigurePagination(this IServiceCollection services)
    {
        return services.AddSingleton<PaginationRequestFilter>();
    }

}
