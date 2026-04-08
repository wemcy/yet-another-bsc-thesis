using System.Diagnostics.CodeAnalysis;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Search;

public class RecipeSearch(string title, AllergenType? includeAllergens, AllergenType? excludeAllergens)
{
    private readonly string title = title;
    private readonly AllergenType? includeAllergens = includeAllergens;
    private readonly AllergenType? excludeAllergens = excludeAllergens;

    public string Title => title;

    public AllergenType? IncludeAllergens => includeAllergens;

    public AllergenType? ExcludeAllergens => excludeAllergens;

    [MemberNotNullWhen(true, nameof(IncludeAllergens))]
    public bool HasIncludeFilter => includeAllergens is not null;

    [MemberNotNullWhen(true, nameof(ExcludeAllergens))]
    public bool HasExcludeFilter => excludeAllergens is not null;
}