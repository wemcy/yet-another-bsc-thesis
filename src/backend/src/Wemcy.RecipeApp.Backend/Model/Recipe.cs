using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Model;

public class Recipe : Entity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required IList<string> Steps { get; set; } = [];
    public required IList<Ingredient> Ingredients { get; set; } = [];
    public required Image? Image { get; set; } = null;
    public virtual required IList<Allergen> Allergens { get; set; } = [];

}
