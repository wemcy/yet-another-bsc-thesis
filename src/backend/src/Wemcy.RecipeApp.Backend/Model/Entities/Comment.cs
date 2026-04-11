namespace Wemcy.RecipeApp.Backend.Model.Entities;

public class Comment : Entity
{
    public virtual Recipe Recipe { get; set; } = null!;

    public required string Content { get; set; }

    public virtual required User User { get; set; } = null!;
}
