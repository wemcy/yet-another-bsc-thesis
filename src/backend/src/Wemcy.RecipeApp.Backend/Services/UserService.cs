using System.Security.Claims;

namespace Wemcy.RecipeApp.Backend.Services
{
    public class UserService(IHttpContextAccessor httpContextAccessor)
    {
        public ClaimsPrincipal GetCurrentUser() {
            return httpContextAccessor?.HttpContext?.User ?? throw new UnauthorizedAccessException();
        }
    }
}
