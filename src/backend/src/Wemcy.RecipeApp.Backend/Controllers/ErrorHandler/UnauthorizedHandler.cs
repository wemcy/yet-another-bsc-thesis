using Microsoft.AspNetCore.Mvc.Filters;
using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;

public class UnauthorizedHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is UnauthorizedAccessException)
        {
            context.HttpContext.Response.StatusCode = 403; // Not Found
            context.Result = new JsonResult(new ErrorResponse() { Message = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
