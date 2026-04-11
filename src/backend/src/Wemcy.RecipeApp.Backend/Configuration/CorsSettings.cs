namespace Wemcy.RecipeApp.Backend.Configuration;

public class CorsSettings
{
    public const string SectionName = "Cors";

    public string[] AllowedOrigins { get; set; } = [];
}
