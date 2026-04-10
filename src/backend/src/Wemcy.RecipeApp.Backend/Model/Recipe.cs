using NpgsqlTypes;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Model;

public class Recipe : Entity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public virtual required AppUser User { get; set; } = null!;
    public required IList<string> Steps { get; set; } = [];
    public required IList<Ingredient> Ingredients { get; set; } = [];
    public required double AverageRating { get; set; }
    public required AllergenType Allergens { get; set; } = AllergenType.None;
    public virtual required Image? Image { get; set; } = null;
    public virtual required IList<Rating> Ratings { get; set; } = [];
    public virtual required IList<Comment> Comments { get; set; } = [];
    public string CreatorDisplayName => User.DisplayName;
    public NpgsqlTsVector TitleSearchVector { get; set; } = null!;
    public void UpdateAverageRating()
    {
        AverageRating = Ratings.Count > 0 ? Ratings.Average(r => r.Value) : 0;
    }

    public Comment GetCommentById(Guid commentId)
    {
        var comment = Comments.SingleOrDefault(c => c.Id == commentId);
        return comment ?? throw new CommentNotFoundExeption(commentId);
    }
    public void Rate(int newRating, AppUser user)
    {
        var rating = Ratings.SingleOrDefault(r => r.User.Id == user.Id);
        if (rating is null)
        {
            Ratings.Add(new Rating() { Value = newRating, User = user });
        }
        else
        {
            rating.Value = newRating;
        }
        UpdateAverageRating();
    }
}
