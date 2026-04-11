using Microsoft.AspNetCore.Mvc.Filters;

namespace Wemcy.RecipeApp.Backend.Pagination;

public class PaginationRequestFilter(IMapper mapper) : IResultFilter
{
    private const string PAGINATION_HEADER_NAME = "X-Pagination";

    protected IMapper Mapper => mapper;

    public void OnResultExecuted(ResultExecutedContext context)
    {
        // Do nothing
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is IPaginatedResult paginatedResult)
        {
            HandlePaginatedResult(context.HttpContext, paginatedResult);
        }
        else if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is IPaginatedResult result)
            {
                HandlePaginatedResult(context.HttpContext, result);
            }
        }
    }


    private static void HandlePaginatedResult(HttpContext httpContext, IPaginatedResult result)
    {
        AddPaginationHeader(httpContext, result);
    }

    private static void AddPaginationHeader(HttpContext httpContext, IPaginatedResult paginationData)
    {
        httpContext.Response.Headers.Append(PAGINATION_HEADER_NAME, JsonConvert.SerializeObject(paginationData));
    }
}
