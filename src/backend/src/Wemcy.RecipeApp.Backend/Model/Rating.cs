using System.ComponentModel.DataAnnotations;

namespace Wemcy.RecipeApp.Backend.Model;

public class Rating : Entity
{
    public virtual Recipe Recipe { get; set; } = null!; // Entity framwork, we need to set this to null, but it will be set when we add the rating to the recipe
    [Range(1, 5)]
    public required int Value { get; set; }
    public virtual required AppUser User { get; set; } = null!;
}
