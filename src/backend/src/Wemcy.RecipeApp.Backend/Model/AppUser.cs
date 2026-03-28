using Microsoft.AspNetCore.Identity;

namespace Wemcy.RecipeApp.Backend.Model;

public class AppUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;
    public DateTimeOffset RegisteredAt { get; set; }
    public virtual required Image? Image { get; set; } = null;

}
