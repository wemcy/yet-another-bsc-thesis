using Microsoft.AspNetCore.Identity;

namespace Wemcy.RecipeApp.Backend.Exceptions;

public class RegistrationExeption(IEnumerable<IdentityError> errors) : Exception
{
    private readonly IEnumerable<IdentityError> _errors = errors;
    public IEnumerable<IdentityError> Errors => _errors;
}
