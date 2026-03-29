using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Wemcy.RecipeApp.Backend.Pagination;

namespace Wemcy.RecipeApp.Backend.Pagination
{
    public class PaginationRequestFilter(IMapper mapper) : IResultFilter
    {
        private const string PAGINATION_HADER_NAME = "X-Pagination";

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


        private void HandlePaginatedResult(HttpContext httpContext, IPaginatedResult result)
        {
            //var paginationDataDto = Mapper.Map<SomeDTO>(result);

            AddPaginationHeader(httpContext, result);
        }

        private void AddPaginationHeader(HttpContext httpContext, IPaginatedResult paginationData)
        {
            httpContext.Response.Headers.Append(PAGINATION_HADER_NAME, JsonConvert.SerializeObject(paginationData));
        }
    }
}
