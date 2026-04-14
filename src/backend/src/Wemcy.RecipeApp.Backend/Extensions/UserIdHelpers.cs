using Microsoft.AspNet.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace Wemcy.RecipeApp.Backend.Extensions;

public static class UserIdHelpers
{

    extension(IIdentity? user)
    {
        public bool TryGetUserId([NotNullWhen(true)] out Guid userId)
        {
            var userID = user.GetUserId();
            if (Guid.TryParse(userID, out userId))
            {
                return true;
            }
            return false;
        }
    }
}
