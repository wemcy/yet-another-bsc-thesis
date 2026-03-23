namespace Wemcy.RecipeApp.Backend.Exceptions;

public class UserNotFoundException(string email) : Exception($"User with email '{email}' was not found.");
