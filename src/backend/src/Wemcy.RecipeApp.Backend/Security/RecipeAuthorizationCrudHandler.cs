using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Security;

public class RecipeAuthorizationCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, Recipe>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Recipe resource)
    {
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
