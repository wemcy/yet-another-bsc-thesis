namespace Wemcy.RecipeApp.Backend.Model.Entities;

[Owned]
public record Ingredient
{
    public required string Name { get; set; }
    public required float Quantity { get; set; }
    public required string UnitOfMeasurement { get; set; }
    public required AllergenType Allergens { get; set; } = AllergenType.None;

}
