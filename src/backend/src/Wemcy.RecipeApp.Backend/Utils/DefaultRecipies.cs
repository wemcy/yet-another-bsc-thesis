using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Utils;

public class DefaultRecipies
{
    public static IEnumerable<Recipe> GetDefaultRecipes()
    {
        yield return new Recipe
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Title = "Spaghetti Carbonara",
            Description = "A classic Italian pasta dish made with eggs, cheese, pancetta, and pepper.",
            Steps =
            [
                "Cook spaghetti according to package instructions.",
                "In a separate pan, cook pancetta until crispy.",
                "In a bowl, whisk together eggs and grated cheese.",
                "Drain spaghetti and return to pot. Mix in pancetta and egg mixture quickly.",
                "Season with pepper and serve immediately."
            ],
            Ingredients = [],
            Allergens = AllergenType.Gluten | AllergenType.Milk | AllergenType.Eggs,
            Comments = [],
            Image = null,
            Ratings = []
        };
    }
}
