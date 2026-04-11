using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace Wemcy.RecipeApp.Backend.Security
{
    public class CommentAuthorizationCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, Comment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Comment resource)
        {
            if (context.User.IsInRole(Roles.Admin) && requirement.Name == Operations.Delete.Name)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
