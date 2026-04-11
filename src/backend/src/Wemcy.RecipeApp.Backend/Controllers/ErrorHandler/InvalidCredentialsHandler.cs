using Microsoft.AspNetCore.Mvc.Filters;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;

public class InvalidCredentialsHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidCredentialsException)
        {
            context.HttpContext.Response.StatusCode = 401; // Unauthorized
            context.Result = new JsonResult(new ErrorResponse() { Message = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
