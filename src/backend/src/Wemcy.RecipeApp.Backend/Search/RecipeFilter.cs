using System.Diagnostics.CodeAnalysis;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Search;

public class RecipeFilter(AllergenType? includeAllergens, AllergenType? excludeAllergens) : IQueryFilter<Recipe>
{
    private readonly AllergenType? includeAllergens = includeAllergens;
    private readonly AllergenType? excludeAllergens = excludeAllergens;
    public AllergenType? IncludeAllergens => includeAllergens;

    public AllergenType? ExcludeAllergens => excludeAllergens;

    [MemberNotNullWhen(true, nameof(IncludeAllergens))]
    public bool HasIncludeFilter => includeAllergens is not null;

    [MemberNotNullWhen(true, nameof(ExcludeAllergens))]
    public bool HasExcludeFilter => excludeAllergens is not null;

    public IQueryable<Recipe> ApplyFilters(IQueryable<Recipe> query)
    {
        return query
            .ConditionalWhere(HasIncludeFilter, x => (x.Allergens & IncludeAllergens) != AllergenType.None)
            .ConditionalWhere(HasExcludeFilter, x => (x.Allergens & ExcludeAllergens) == AllergenType.None);
    }
}
