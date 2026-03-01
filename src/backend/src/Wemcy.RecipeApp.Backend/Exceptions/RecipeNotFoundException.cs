namespace Wemcy.RecipeApp.Backend.Exceptions
{
    public class RecipeNotFoundException : Exception
    {
        public Guid RecipeId { get; }
        public RecipeNotFoundException(Guid recipeId) : base("Recipe not found.")
        {
            RecipeId = recipeId;
        }
    }
}
