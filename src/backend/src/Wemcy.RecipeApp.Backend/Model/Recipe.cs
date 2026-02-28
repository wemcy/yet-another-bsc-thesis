using Wemcy.RecipeApp.Backend.Api.Models;

namespace Wemcy.RecipeApp.Backend.Model;

public class Recipe : Entity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required List<string> Steps { get; set; }
    //public List<Ingredient> ingreditents { get; set; }
    public virtual required IList<Allergen> Allergens { get; set; } = [];

}
