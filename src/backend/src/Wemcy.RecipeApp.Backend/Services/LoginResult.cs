namespace Wemcy.RecipeApp.Backend.Services;

public record LoginResult(Guid UserId, string Email, string DisplayName);
