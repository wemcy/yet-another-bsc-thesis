namespace Wemcy.RecipeApp.Backend.Exceptions;

public class ImageNotFoundException : EntityNotFoundExeption
{
    public ImageNotFoundException() : base("Image not found.")
    {
    }
    public ImageNotFoundException(string message) : base(message)
    {
    }
}
