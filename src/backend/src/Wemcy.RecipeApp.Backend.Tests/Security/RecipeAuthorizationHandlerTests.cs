using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Security;

namespace Wemcy.RecipeApp.Backend.Tests.Security;

public class RecipeAuthorizationHandlerTests
{
    private readonly RecipeAuthorizationHandler _handler = new();

    private static ClaimsPrincipal CreateUser(Guid userId, string? role = null)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId.ToString()) };
        if (role is not null)
            claims.Add(new Claim(ClaimTypes.Role, role));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: "Test"));
    }

    private static Recipe CreateRecipe(User? owner = null) => new()
    {
        Title = "Test Recipe",
        Description = "Test",
        User = owner,
        Steps = [],
        Ingredients = [],
        Allergens = AllergenType.None,
        Image = null,
        Ratings = [],
        Comments = []
    };

    [Theory]
    [MemberData(nameof(WriteOperations))]
    public async Task HandleAsync_AdminUser_SucceedsForAllWriteOperations(OperationAuthorizationRequirement operation)
    {
        var admin = CreateUser(Guid.NewGuid(), Roles.Admin);
        var context = new AuthorizationHandlerContext([operation], admin, CreateRecipe());

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(WriteOperations))]
    public async Task HandleAsync_RecipeOwner_SucceedsForOwnRecipe(OperationAuthorizationRequirement operation)
    {
        var ownerId = Guid.NewGuid();
        var owner = CreateUser(ownerId);
        var recipe = CreateRecipe(new User { Id = ownerId, Image = null });
        var context = new AuthorizationHandlerContext([operation], owner, recipe);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(WriteOperations))]
    public async Task HandleAsync_NonOwner_DoesNotSucceed(OperationAuthorizationRequirement operation)
    {
        var recipeOwner = new User { Id = Guid.NewGuid(), Image = null };
        var otherUser = CreateUser(Guid.NewGuid());
        var recipe = CreateRecipe(recipeOwner);
        var context = new AuthorizationHandlerContext([operation], otherUser, recipe);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(WriteOperations))]
    public async Task HandleAsync_UnauthenticatedUser_DoesNotSucceed(OperationAuthorizationRequirement operation)
    {
        var anonymous = new ClaimsPrincipal();
        var context = new AuthorizationHandlerContext([operation], anonymous, CreateRecipe());

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
    }

    public static TheoryData<OperationAuthorizationRequirement> WriteOperations() =>
        new() { Operations.Create, Operations.Update, Operations.Delete };
}
