using Microsoft.AspNet.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Wemcy.RecipeApp.Backend.Extensions;


public static class UserIdHelpers
{
    
    public static bool TryGetUserId(this System.Security.Principal.IIdentity? user, [NotNullWhen(true)] out Guid userId)
    {
        var userID = user.GetUserId();
        if (Guid.TryParse(userID, out userId))
        {
            return true;
        }
        return false;
    }
}
