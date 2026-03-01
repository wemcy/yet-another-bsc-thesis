namespace Wemcy.RecipeApp.Backend.Exceptions
{
    public class RecipeNotFoundException(Guid recipeId) : Exception("Recipe not found.")
    {
        public Guid RecipeId { get; } = recipeId;
    }
}
