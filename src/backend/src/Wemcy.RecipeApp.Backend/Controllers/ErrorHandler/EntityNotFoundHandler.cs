using Microsoft.AspNetCore.Mvc.Filters;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;

public class EntityNotFoundHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is EntityNotFoundExeption)
        {
            context.HttpContext.Response.StatusCode = 404; // Not Found
            context.Result = new JsonResult(new ErrorResponse() { Message = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
