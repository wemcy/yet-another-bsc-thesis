namespace Wemcy.RecipeApp.Backend.Exceptions;

public class UnknowRoleExeption(string roleName) : Exception($"The role '{roleName}' is not recognized.")
{
}
