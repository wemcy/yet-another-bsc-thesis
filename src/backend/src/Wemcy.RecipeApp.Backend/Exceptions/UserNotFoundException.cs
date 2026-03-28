namespace Wemcy.RecipeApp.Backend.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string email) : base($"User with email '{email}' was not found.")
    {
    }

    public UserNotFoundException(Guid userId) : base($"User with ID '{userId}' was not found.")
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public UserNotFoundException() : base("User was not found.")
    {
    }
}
