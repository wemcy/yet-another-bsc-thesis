namespace Wemcy.RecipeApp.Backend.Configuration;

public class AdminSettings
{
    public const string SectionName = "Admin";

    public string Email { get; set; } = "admin@recipe-app.local";
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = "Administrator";
}
