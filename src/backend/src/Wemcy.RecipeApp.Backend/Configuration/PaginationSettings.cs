namespace Wemcy.RecipeApp.Backend.Configuration;

public class PaginationSettings
{
    public const string SectionName = "Pagination";

    public int DefaultPageSize { get; set; } = 25;
}
