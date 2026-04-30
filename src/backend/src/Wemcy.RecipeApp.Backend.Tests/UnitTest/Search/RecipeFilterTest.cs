using FluentAssertions;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Tests.UnitTest.Search;

public class RecipeFilterTest
{
    [Fact]
    public void ApplyFilters_IncludeFilter_ReturnsRecipesContainingAllSelectedAllergens()
    {
        var recipes = new[]
        {
            CreateRecipe("Gluten only", AllergenType.Gluten),
            CreateRecipe("Milk only", AllergenType.Milk),
            CreateRecipe("Gluten and milk", AllergenType.Gluten | AllergenType.Milk),
            CreateRecipe("Gluten milk and eggs", AllergenType.Gluten | AllergenType.Milk | AllergenType.Eggs),
        }.AsQueryable();

        var filter = new RecipeFilter(AllergenType.Gluten | AllergenType.Milk, null);

        var result = filter.ApplyFilters(recipes).Select(recipe => recipe.Title).ToList();

        result.Should().Equal("Gluten and milk", "Gluten milk and eggs");
    }

    [Fact]
    public void ApplyFilters_ExcludeFilter_ReturnsRecipesWithoutSelectedAllergens()
    {
        var recipes = new[]
        {
            CreateRecipe("Gluten only", AllergenType.Gluten),
            CreateRecipe("Milk only", AllergenType.Milk),
            CreateRecipe("No allergens", AllergenType.None),
        }.AsQueryable();

        var filter = new RecipeFilter(null, AllergenType.Gluten);

        var result = filter.ApplyFilters(recipes).Select(recipe => recipe.Title).ToList();

        result.Should().Equal("Milk only", "No allergens");
    }

    private static Recipe CreateRecipe(string title, AllergenType allergens)
    {
        return new Recipe
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = title,
            Allergens = allergens,
            Steps = [],
            Ingredients = [],
            Image = null,
            Ratings = [],
            Comments = [],
        };
    }
}
