using FluentAssertions;
using NSubstitute;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Tests.Services;

public class ShowcaseRecipeServiceTests
{
    private readonly IRecipeService _recipeService = Substitute.For<IRecipeService>();
    private readonly IRecipeShowcaseRepository _showcaseRepo = Substitute.For<IRecipeShowcaseRepository>();
    private readonly ShowcaseRecipeService _sut;

    public ShowcaseRecipeServiceTests()
    {
        _sut = new ShowcaseRecipeService(_recipeService, _showcaseRepo);
    }

    private static Recipe NewRecipe() => new()
    {
        Title = "Test",
        Description = "Test",
        Steps = [],
        Ingredients = [],
        Allergens = AllergenType.None,
        Image = null,
        Ratings = [],
        Comments = []
    };

    private static RecipeShowcase NewShowcase() => new() { ShowcaseRecipeIds = [], FeaturedRecipe = null };

    // ── GetFeaturedRecipeAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetFeaturedRecipeAsync_DelegatesToRepository()
    {
        var recipe = NewRecipe();
        _showcaseRepo.GetFeaturedRecipe().Returns(recipe);

        var result = await _sut.GetFeaturedRecipeAsync();

        result.Should().Be(recipe);
    }

    // ── GetShowcaseRecipesAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetShowcaseRecipesAsync_FetchesIdsThenDelegatesToRecipeService()
    {
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var recipes = new List<Recipe> { NewRecipe(), NewRecipe() };
        _showcaseRepo.GetShowcaseRecipeIds().Returns(ids);
        _recipeService.ListResipesAsAsync<Recipe>(Arg.Any<IQueryFilter<Recipe>>()).Returns(recipes);

        var result = await _sut.GetShowcaseRecipesAsync<Recipe>();

        result.Should().BeEquivalentTo(recipes);
        await _showcaseRepo.Received(1).GetShowcaseRecipeIds();
    }

    // ── SetFeaturedRecipeAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task SetFeaturedRecipeAsync_UpdatesFeaturedRecipeAndSaves()
    {
        var recipeId = Guid.NewGuid();
        var recipe = NewRecipe();
        var showcase = NewShowcase();
        _recipeService.GetRecipeByIdAsync(recipeId).Returns(recipe);
        _showcaseRepo.GetRecipeShowcase().Returns(showcase);

        await _sut.SetFeaturedRecipeAsync(recipeId);

        showcase.FeaturedRecipe.Should().Be(recipe);
        await _showcaseRepo.Received(1).SaveAsync();
    }

    // ── UpdateShowcaseRecipesAsync ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateShowcaseRecipesAsync_SetsRandomIdsOnShowcaseAndSaves()
    {
        var randomIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var showcase = NewShowcase();
        _recipeService.GetRandomRecipesGuids(6).Returns(randomIds);
        _showcaseRepo.GetRecipeShowcase().Returns(showcase);

        await _sut.UpdateShowcaseRecipesAsync();

        showcase.ShowcaseRecipeIds.Should().BeEquivalentTo(randomIds);
        await _showcaseRepo.Received(1).SaveAsync();
    }
}
