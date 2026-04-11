using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Security;

public class UserAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, User>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, User resource)
    {
        // Admin users can manage every user.
        if (context.User.IsInRole(Roles.Admin) &&
            (requirement.Name == Operations.Create.Name ||
             requirement.Name == Operations.Read.Name ||
             requirement.Name == Operations.Update.Name ||
             requirement.Name == Operations.Delete.Name))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (context.User.Identity.TryGetUserId(out Guid id))
        {
            if (id == resource.Id &&
                (requirement.Name == Operations.Update.Name ||
                 requirement.Name == Operations.Read.Name ||
                 requirement.Name == Operations.Delete.Name))
                context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
