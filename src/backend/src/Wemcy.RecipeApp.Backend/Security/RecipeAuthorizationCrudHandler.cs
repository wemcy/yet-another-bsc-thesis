using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Security
{
    public class RecipeAuthorizationCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, Recipe>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Recipe resource)
        {
            if (Guid.Parse(context.User.Identity?.Name) == resource.UserId &&
            requirement.Name == Operations.Update.Name)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
