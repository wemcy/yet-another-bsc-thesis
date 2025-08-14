namespace Wemcy.RecipeApp.Backend.Model;

public class Recipe
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    //public List<Step> steps { get; set; }
    //public List<Ingredient> ingreditents { get; set; }
    //public List<Allergen> allergens { get; set; }

}
