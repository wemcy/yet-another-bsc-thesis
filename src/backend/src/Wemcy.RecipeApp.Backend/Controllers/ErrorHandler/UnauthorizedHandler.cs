using Microsoft.AspNetCore.Mvc.Filters;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;

public class UnauthorizedHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is UnauthorizedAccessException)
        {
            context.HttpContext.Response.StatusCode = 403; // Not Found
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { error = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
