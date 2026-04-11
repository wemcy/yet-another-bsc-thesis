using Microsoft.AspNetCore.Mvc.Filters;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;

public class RegistrationErrorHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is RegistrationExeption registrationExeption)
        {
            context.HttpContext.Response.StatusCode = 400; // Bad Request
            context.Result = new JsonResult( registrationExeption.Errors );
            context.ExceptionHandled = true;
        }
    }
}
