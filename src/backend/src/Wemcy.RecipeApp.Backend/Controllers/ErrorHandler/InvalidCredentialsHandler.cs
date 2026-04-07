using Microsoft.AspNetCore.Mvc.Filters;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;

public class InvalidCredentialsHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidCredentialsException)
        {
            context.HttpContext.Response.StatusCode = 401; // Unauthorized
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { error = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
