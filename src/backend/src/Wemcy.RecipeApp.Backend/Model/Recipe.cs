using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Model;

public class Recipe : Entity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required IList<string> Steps { get; set; } = [];
    public required IList<Ingredient> Ingredients { get; set; } = [];
    public required double AverageRating { get; set; }
    public required AllergenType Allergens { get; set; } = AllergenType.None;
    public virtual required Image? Image { get; set; } = null;
    public virtual required IList<Rating> Ratings { get; set; } = [];
    public virtual required IList<Comment> Comments { get; set; } = [];

    public void UpdateAverageRating()
    {
        AverageRating = Ratings.Count > 0 ? Ratings.Average(r => r.Value) : 0;
    }

    public void Rate(int rating)
    {
        Ratings.Add(new Rating() { Value = rating });
        UpdateAverageRating();
    }
}
