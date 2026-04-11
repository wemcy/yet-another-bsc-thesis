namespace Wemcy.RecipeApp.Backend.Model.Entities;

public class RecipeShowcase
{
    public static readonly int SingletonId = 1;

    [Key]
    public int Id { get; set; }

    public Guid[] ShowcaseRecipeIds { get; set; } = [];

    public virtual Recipe? FeaturedRecipe { get; set; }
}
