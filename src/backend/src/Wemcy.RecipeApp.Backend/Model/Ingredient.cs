using Microsoft.EntityFrameworkCore;

namespace Wemcy.RecipeApp.Backend.Model;

[Owned]
public record Ingredient
{
    public required string Name { get; set; }
    public required double Quantity { get; set; }
    public required string UnitOfMeasurement { get; set; }
}