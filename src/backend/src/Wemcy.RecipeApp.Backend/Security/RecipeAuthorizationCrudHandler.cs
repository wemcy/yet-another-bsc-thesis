using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Security;

public class RecipeAuthorizationCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, Recipe>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Recipe resource)
    {
        // Admin users can manage every recipe.
        if (context.User.IsInRole(Roles.Admin) &&
            (requirement.Name == Operations.Create.Name ||
             requirement.Name == Operations.Update.Name ||
             requirement.Name == Operations.Delete.Name))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userIdValue = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdValue is null || !Guid.TryParse(userIdValue, out var userId))
            return Task.CompletedTask;

        if (resource.User is not null && userId == resource.User.Id &&
            (requirement.Name == Operations.Create.Name ||
             requirement.Name == Operations.Update.Name ||
             requirement.Name == Operations.Delete.Name))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
public class AppUserAuthorizationCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, AppUser>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, AppUser resource)
    {
        // Admin users can manage every user.
        if (context.User.IsInRole(Roles.Admin) &&
            (requirement.Name == Operations.Create.Name ||
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
                 requirement.Name == Operations.Delete.Name))
                context.Succeed(requirement);
        } else {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}