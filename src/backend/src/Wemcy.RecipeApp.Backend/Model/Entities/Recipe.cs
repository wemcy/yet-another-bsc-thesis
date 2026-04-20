using NpgsqlTypes;
using Wemcy.RecipeApp.Backend.Exceptions;

namespace Wemcy.RecipeApp.Backend.Model.Entities;

public class Recipe : Entity
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public virtual User? User { get; set; } = null!;

    public required IList<string> Steps { get; set; } = [];

    public required IList<Ingredient> Ingredients { get; set; } = [];

    public double AverageRating { get; set; } = 0.0;

    public required AllergenType Allergens { get; set; } = AllergenType.None;

    public virtual required Image? Image { get; set; } = null;

    public virtual required IList<Rating> Ratings { get; set; } = [];

    public virtual required IList<Comment> Comments { get; set; } = [];

    public string CreatorDisplayName => User?.DisplayName ?? "Unknown";
    public Guid? CreatorAuthorId => User?.Id;


    public NpgsqlTsVector TitleSearchVector { get; set; } = null!;

    public void UpdateAverageRating()
    {
        AverageRating = Ratings.Count > 0 ? Ratings.Average(r => r.Value) : 0;
    }

    public void UpdateAllergens()
    {
        Allergens = Ingredients.Aggregate(AllergenType.None, (current, ingredient) => current | ingredient.Allergens);
    }

    public Comment GetCommentById(Guid commentId)
    {
        var comment = Comments.SingleOrDefault(c => c.Id == commentId);
        return comment ?? throw new CommentNotFoundExeption(commentId);
    }

    public void Rate(int newRating, User user)
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
    public override void OnSave()
    {
        UpdateAllergens();
        UpdateAverageRating();
    }
}
