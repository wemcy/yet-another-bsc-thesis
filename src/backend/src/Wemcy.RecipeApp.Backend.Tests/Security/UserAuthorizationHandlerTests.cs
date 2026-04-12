using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Tests.Security;

public class UserAuthorizationHandlerTests
{
    private readonly UserAuthorizationHandler _handler = new();

    private static ClaimsPrincipal CreateUser(Guid userId, string? role = null)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId.ToString()) };
        if (role is not null)
            claims.Add(new Claim(ClaimTypes.Role, role));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Test"));
    }

    private static User CreateUserEntity(Guid? id = null) => new() { Id = id ?? Guid.NewGuid(), Image = null };

    [Theory]
    [MemberData(nameof(AllOperations))]
    public async Task HandleAsync_AdminUser_SucceedsForAllOperations(OperationAuthorizationRequirement operation)
    {
        var admin = CreateUser(Guid.NewGuid(), Roles.Admin);
        var target = CreateUserEntity();
        var context = new AuthorizationHandlerContext([operation], admin, target);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(SelfAllowedOperations))]
    public async Task HandleAsync_Owner_SucceedsForOwnProfile(OperationAuthorizationRequirement operation)
    {
        var ownerId = Guid.NewGuid();
        var owner = CreateUser(ownerId);
        var target = CreateUserEntity(ownerId);
        var context = new AuthorizationHandlerContext([operation], owner, target);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleAsync_Owner_CannotCreate()
    {
        var ownerId = Guid.NewGuid();
        var owner = CreateUser(ownerId);
        var target = CreateUserEntity(ownerId);
        var context = new AuthorizationHandlerContext([Operations.Create], owner, target);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(AllOperations))]
    public async Task HandleAsync_NonOwner_DoesNotSucceed(OperationAuthorizationRequirement operation)
    {
        var otherUser = CreateUser(Guid.NewGuid());
        var target = CreateUserEntity(Guid.NewGuid());
        var context = new AuthorizationHandlerContext([operation], otherUser, target);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(AllOperations))]
    public async Task HandleAsync_UnauthenticatedUser_Fails(OperationAuthorizationRequirement operation)
    {
        var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        var target = CreateUserEntity();
        var context = new AuthorizationHandlerContext([operation], anonymous, target);

        await _handler.HandleAsync(context);

        context.HasFailed.Should().BeTrue();
    }

    public static TheoryData<OperationAuthorizationRequirement> AllOperations => new()
    {
        Operations.Create,
        Operations.Read,
        Operations.Update,
        Operations.Delete
    };

    public static TheoryData<OperationAuthorizationRequirement> SelfAllowedOperations => new()
    {
        Operations.Read,
        Operations.Update,
        Operations.Delete
    };
}
