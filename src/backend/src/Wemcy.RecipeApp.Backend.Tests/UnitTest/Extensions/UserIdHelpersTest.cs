using Microsoft.AspNet.Identity;
using Moq;
using Wemcy.RecipeApp.Backend.Extensions;
using System.Security.Principal;
using System.Security.Claims;

namespace Wemcy.RecipeApp.Backend.Tests.UnitTest.Extensions;

public class UserIdHelpersTest
{
    [Fact]
    public void TryGetUserId_Should_Return_True_For_Valid_Guid()
    {

        // Arrange
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var identity = new ClaimsIdentity([new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId.ToString())]);
        // Act
        var result = identity.TryGetUserId(out var extractedUserId);
        // Assert
        Assert.True(result);
        Assert.Equal(userId, extractedUserId);
    }

}
