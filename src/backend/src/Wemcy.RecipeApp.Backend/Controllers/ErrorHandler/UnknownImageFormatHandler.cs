using Microsoft.AspNetCore.Mvc.Filters;
using SixLabors.ImageSharp;
using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Controllers.ErrorHandler
{
    public class UnknownImageFormatHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnknownImageFormatException)
            {
                context.HttpContext.Response.StatusCode = 415; // Unsupported Media Type
                context.Result = new JsonResult(new ErrorResponse() { Message = context.Exception.Message });
                context.ExceptionHandled = true;
            }
        }
    }
}
