namespace Wemcy.RecipeApp.Backend.Model.Entities;

public class Rating : Entity
{
    public virtual Recipe Recipe { get; set; } = null!;
    [Range(1, 5)]
    public required int Value { get; set; }
    public virtual required User User { get; set; } = null!;
}
