using FluentAssertions;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model.Entities;

namespace Wemcy.RecipeApp.Backend.Tests.Model;

public class RecipeTests
{
    private static Recipe NewRecipe(IList<Rating>? ratings = null, IList<Comment>? comments = null) => new()
    {
        Title = "Test Recipe",
        Description = "Test",
        Steps = [],
        Ingredients = [],
        Allergens = AllergenType.None,
        Image = null,
        Ratings = ratings ?? [],
        Comments = comments ?? []
    };

    private static User NewUser() => new() { Id = Guid.NewGuid(), Image = null };

    // ── Rate() ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Rate_NewUser_AddsRatingAndUpdatesAverage()
    {
        var recipe = NewRecipe();
        var user = NewUser();

        recipe.Rate(4, user);

        recipe.Ratings.Should().ContainSingle(r => r.Value == 4 && r.User == user);
        recipe.AverageRating.Should().Be(4);
    }

    [Fact]
    public void Rate_ExistingUser_UpdatesRatingAndAverage()
    {
        var user = NewUser();
        var recipe = NewRecipe(ratings: [new Rating { Value = 3, User = user }]);

        recipe.Rate(5, user);

        recipe.Ratings.Should().ContainSingle(r => r.Value == 5 && r.User == user);
        recipe.AverageRating.Should().Be(5);
    }

    [Fact]
    public void Rate_MultipleUsers_AverageIsCorrect()
    {
        var userA = NewUser();
        var userB = NewUser();
        var recipe = NewRecipe();

        recipe.Rate(4, userA);
        recipe.Rate(2, userB);

        recipe.AverageRating.Should().Be(3);
    }

    // ── UpdateAverageRating() ────────────────────────────────────────────────────

    [Fact]
    public void UpdateAverageRating_NoRatings_ReturnsZero()
    {
        var recipe = NewRecipe();

        recipe.UpdateAverageRating();

        recipe.AverageRating.Should().Be(0);
    }

    [Fact]
    public void UpdateAverageRating_WithRatings_ReturnsCorrectAverage()
    {
        var recipe = NewRecipe(ratings:
        [
            new Rating { Value = 2, User = NewUser() },
            new Rating { Value = 4, User = NewUser() }
        ]);

        recipe.UpdateAverageRating();

        recipe.AverageRating.Should().Be(3);
    }

    // ── GetCommentById() ─────────────────────────────────────────────────────────

    [Fact]
    public void GetCommentById_ExistingId_ReturnsComment()
    {
        var comment = new Comment { Id = Guid.NewGuid(), Content = "Yummy!", User = NewUser() };
        var recipe = NewRecipe(comments: [comment]);

        var result = recipe.GetCommentById(comment.Id);

        result.Should().Be(comment);
    }

    [Fact]
    public void GetCommentById_NonExistingId_ThrowsCommentNotFoundException()
    {
        var recipe = NewRecipe();

        var act = () => recipe.GetCommentById(Guid.NewGuid());

        act.Should().Throw<CommentNotFoundExeption>();
    }
}
