using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Tests.Security;

public class CommentAuthorizationCrudHandlerTests
{
    private readonly CommentAuthorizationCrudHandler _handler = new();

    private static ClaimsPrincipal CreateUser(string? role = null)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()) };
        if (role is not null)
            claims.Add(new Claim(ClaimTypes.Role, role));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Test"));
    }

    private static Comment CreateComment() => new()
    {
        Content = "Test comment",
        User = new User { Id = Guid.NewGuid(), Image = null }
    };

    [Fact]
    public async Task HandleAsync_AdminUser_Delete_Succeeds()
    {
        var admin = CreateUser(Roles.Admin);
        var context = new AuthorizationHandlerContext([Operations.Delete], admin, CreateComment());

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(NonDeleteOperations))]
    public async Task HandleAsync_AdminUser_NonDeleteOperation_DoesNotSucceed(OperationAuthorizationRequirement operation)
    {
        var admin = CreateUser(Roles.Admin);
        var context = new AuthorizationHandlerContext([operation], admin, CreateComment());

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_RegularUser_Delete_DoesNotSucceed()
    {
        var user = CreateUser();
        var context = new AuthorizationHandlerContext([Operations.Delete], user, CreateComment());

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Fact]
    public async Task HandleAsync_AnonymousUser_Delete_DoesNotSucceed()
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext([Operations.Delete], anonymous, CreateComment());

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    public static TheoryData<OperationAuthorizationRequirement> NonDeleteOperations => new()
    {
        Operations.Create,
        Operations.Read,
        Operations.Update
    };
}
