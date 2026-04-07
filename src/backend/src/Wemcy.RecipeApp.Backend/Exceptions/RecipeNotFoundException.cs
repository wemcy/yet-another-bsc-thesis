namespace Wemcy.RecipeApp.Backend.Exceptions;

public class RecipeNotFoundException(Guid recipeId) : EntityNotFoundExeption("Recipe not found.")
{
    public Guid RecipeId { get; } = recipeId;
}
