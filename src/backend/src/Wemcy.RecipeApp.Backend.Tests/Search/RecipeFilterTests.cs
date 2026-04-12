using FluentAssertions;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Tests.Search;

public class RecipeFilterTests
{
    private static Recipe NewRecipe(AllergenType allergens) => new()
    {
        Title = "Test",
        Description = "Test",
        Steps = [],
        Ingredients = [],
        Allergens = allergens,
        Image = null,
        Ratings = [],
        Comments = []
    };

    private static IQueryable<Recipe> NewQuery(params AllergenType[] allergens)
        => allergens.Select(NewRecipe).AsQueryable();

    [Fact]
    public void ApplyFilters_NoFilters_ReturnsAll()
    {
        var query = NewQuery(AllergenType.None, AllergenType.Gluten, AllergenType.Milk);
        var filter = new RecipeFilter(null, null);

        var result = filter.ApplyFilters(query).ToList();

        result.Should().HaveCount(3);
    }

    [Fact]
    public void ApplyFilters_IncludeFilter_ReturnsOnlyMatchingRecipes()
    {
        var query = NewQuery(AllergenType.None, AllergenType.Gluten, AllergenType.Milk);
        var filter = new RecipeFilter(AllergenType.Gluten, null);

        var result = filter.ApplyFilters(query).ToList();

        result.Should().ContainSingle(r => r.Allergens == AllergenType.Gluten);
    }

    [Fact]
    public void ApplyFilters_ExcludeFilter_FiltersOutMatchingRecipes()
    {
        var query = NewQuery(AllergenType.None, AllergenType.Gluten, AllergenType.Milk);
        var filter = new RecipeFilter(null, AllergenType.Gluten);

        var result = filter.ApplyFilters(query).ToList();

        result.Should().HaveCount(2).And.NotContain(r => r.Allergens == AllergenType.Gluten);
    }

    [Fact]
    public void ApplyFilters_BothFilters_IncludeAndExcludeCombineCorrectly()
    {
        // Gluten only → passes (has gluten, no milk)
        // Milk only   → fails (no gluten)
        // Both        → fails (has gluten but also has milk → excluded)
        // None        → fails (no gluten)
        var query = NewQuery(
            AllergenType.None,
            AllergenType.Gluten,
            AllergenType.Milk,
            AllergenType.Gluten | AllergenType.Milk);
        var filter = new RecipeFilter(AllergenType.Gluten, AllergenType.Milk);

        var result = filter.ApplyFilters(query).ToList();

        result.Should().ContainSingle(r => r.Allergens == AllergenType.Gluten);
    }
}
