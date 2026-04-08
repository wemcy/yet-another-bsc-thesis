using Microsoft.AspNetCore.Identity;

namespace Wemcy.RecipeApp.Backend.Services;

public record RegisterResult(bool Succeeded, IEnumerable<IdentityError> Errors);
