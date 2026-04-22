using NpgsqlTypes;

namespace Wemcy.RecipeApp.Backend.Model.Entities;

public class IngredientSuggestion: Entity
{
    public required string Name { get; set; }
    public AllergenType Allergens { get; set; }
    public NpgsqlTsVector NameSearchVector { get; set; } = null!;
}
