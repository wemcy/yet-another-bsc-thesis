using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using NSubstitute;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;
using CreateRecipeRequest = Wemcy.RecipeApp.Backend.Api.Models.CreateRecipeRequest;

namespace Wemcy.RecipeApp.Backend.Tests.Services;

public class RecipeServiceTests
{
    private readonly IRecipeRepository _recipeRepository = Substitute.For<IRecipeRepository>();
    private readonly IImageService _imageService = Substitute.For<IImageService>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IUserService _userService = Substitute.For<IUserService>();
    private readonly RecipeService _sut;

    public RecipeServiceTests()
    {
        _sut = new RecipeService(_recipeRepository, _imageService, _mapper, _userService);
    }

    private static Recipe NewRecipe() => new()
    {
        Title = "Pasta",
        Description = "Simple pasta",
        Steps = [],
        Ingredients = [],
        Allergens = AllergenType.None,
        Image = null,
        Ratings = [],
        Comments = []
    };

    [Fact]
    public async Task CreateRecipeAsync_SetsCurrentUserOnRecipe()
    {
        var currentUser = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        _userService.GetCurrentUserAsync().Returns(currentUser);
        _recipeRepository.CreateRecipeAsync(recipe).Returns(recipe);

        await _sut.CreateRecipeAsync(recipe);

        recipe.User.Should().Be(currentUser);
    }

    [Fact]
    public async Task CreateRecipeAsync_ChecksAuthorizationBeforePersisting()
    {
        var currentUser = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        _userService.GetCurrentUserAsync().Returns(currentUser);
        _recipeRepository.CreateRecipeAsync(recipe).Returns(recipe);

        await _sut.CreateRecipeAsync(recipe);

        await _userService.Received(1)
            .EnsureCurrentUserCanAsync(
                Arg.Is<OperationAuthorizationRequirement>(r => r.Name == Operations.Create.Name),
                recipe);
    }

    [Fact]
    public async Task CreateRecipeAsync_PersistsRecipeToRepository()
    {
        var currentUser = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        _userService.GetCurrentUserAsync().Returns(currentUser);
        _recipeRepository.CreateRecipeAsync(recipe).Returns(recipe);

        var result = await _sut.CreateRecipeAsync(recipe);

        await _recipeRepository.Received(1).CreateRecipeAsync(recipe);
        result.Should().Be(recipe);
    }

    [Fact]
    public async Task CreateRecipeAsync_WhenAuthorizationFails_DoesNotPersist()
    {
        var currentUser = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        _userService.GetCurrentUserAsync().Returns(currentUser);
        _userService
            .EnsureCurrentUserCanAsync(Arg.Any<OperationAuthorizationRequirement>(), recipe)
            .Returns(Task.FromException(new UnauthorizedAccessException()));

        var act = async () => await _sut.CreateRecipeAsync(recipe);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        await _recipeRepository.DidNotReceive().CreateRecipeAsync(Arg.Any<Recipe>());
    }

    // ── RateRecipeAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task RateRecipeAsync_AddsNewRatingAndSaves()
    {
        var user = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        _userService.GetCurrentUserAsync().Returns(user);
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);

        await _sut.RateRecipeAsync(recipe.Id, 4);

        recipe.Ratings.Should().ContainSingle(r => r.Value == 4 && r.User == user);
        await _recipeRepository.Received(1).SaveAsync();
    }

    [Fact]
    public async Task RateRecipeAsync_UpdatesExistingRatingForSameUser()
    {
        var user = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        recipe.Ratings.Add(new Rating { Value = 3, User = user });
        _userService.GetCurrentUserAsync().Returns(user);
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);

        await _sut.RateRecipeAsync(recipe.Id, 5);

        recipe.Ratings.Should().ContainSingle(r => r.Value == 5 && r.User == user);
        await _recipeRepository.Received(1).SaveAsync();
    }

    // ── AddCommentAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task AddCommentAsync_AddsCommentWithContentAndUser_AndSaves()
    {
        var user = new User { Id = Guid.NewGuid(), Image = null };
        var recipe = NewRecipe();
        _userService.GetCurrentUserAsync().Returns(user);
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);

        await _sut.AddCommentAsync(recipe.Id, "Great recipe!");

        recipe.Comments.Should().ContainSingle(c => c.Content == "Great recipe!" && c.User == user);
        await _recipeRepository.Received(1).SaveAsync();
    }

    // ── DeleteRecipeByIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task DeleteRecipeByIdAsync_ChecksAuthorizationAndDeletesAndSaves()
    {
        var recipe = NewRecipe();
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);

        await _sut.DeleteRecipeByIdAsync(recipe.Id);

        await _userService.Received(1)
            .EnsureCurrentUserCanAsync(
                Arg.Is<OperationAuthorizationRequirement>(r => r.Name == Operations.Delete.Name),
                recipe);
        _recipeRepository.Received(1).DeleteRecipe(recipe);
        await _recipeRepository.Received(1).SaveAsync();
    }

    [Fact]
    public async Task DeleteRecipeByIdAsync_WhenAuthorizationFails_DoesNotDelete()
    {
        var recipe = NewRecipe();
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);
        _userService
            .EnsureCurrentUserCanAsync(Arg.Any<OperationAuthorizationRequirement>(), recipe)
            .Returns(Task.FromException(new UnauthorizedAccessException()));

        var act = async () => await _sut.DeleteRecipeByIdAsync(recipe.Id);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        _recipeRepository.DidNotReceive().DeleteRecipe(Arg.Any<Recipe>());
        await _recipeRepository.DidNotReceive().SaveAsync();
    }

    // ── UpdateImageByIdAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateImageByIdAsync_AuthorizesUpdatesImageAndSaves()
    {
        var recipe = NewRecipe();
        var imageStream = new MemoryStream();
        var newImage = new Image { Name = "photo", Extenstion = "jpg" };
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);
        _imageService.CreateImage(imageStream, "photo").Returns(newImage);

        await _sut.UpdateImageByIdAsync(recipe.Id, imageStream, "photo");

        await _userService.Received(1)
            .EnsureCurrentUserCanAsync(
                Arg.Is<OperationAuthorizationRequirement>(r => r.Name == Operations.Update.Name),
                recipe);
        recipe.Image.Should().Be(newImage);
        await _recipeRepository.Received(1).SaveAsync();
    }

    [Fact]
    public async Task UpdateImageByIdAsync_WhenAuthorizationFails_DoesNotSave()
    {
        var recipe = NewRecipe();
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);
        _userService
            .EnsureCurrentUserCanAsync(Arg.Any<OperationAuthorizationRequirement>(), recipe)
            .Returns(Task.FromException(new UnauthorizedAccessException()));

        var act = async () => await _sut.UpdateImageByIdAsync(recipe.Id, new MemoryStream(), "photo");

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        await _recipeRepository.DidNotReceive().SaveAsync();
    }

    // ── UpdateRecipeAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateRecipeAsync_ChecksAuthorizationMapsAndSaves()
    {
        var recipe = NewRecipe();
        var request = new CreateRecipeRequest { Title = "Updated", Steps = [], Ingredients = [], Allergens = [] };
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);

        var result = await _sut.UpdateRecipeAsync(recipe.Id, request);

        await _userService.Received(1)
            .EnsureCurrentUserCanAsync(
                Arg.Is<OperationAuthorizationRequirement>(r => r.Name == Operations.Update.Name),
                recipe);
        _mapper.Received(1).Map(request, recipe);
        await _recipeRepository.Received(1).SaveAsync();
        result.Should().Be(recipe);
    }

    [Fact]
    public async Task UpdateRecipeAsync_WhenAuthorizationFails_DoesNotSave()
    {
        var recipe = NewRecipe();
        var request = new CreateRecipeRequest { Title = "Updated", Steps = [], Ingredients = [], Allergens = [] };
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);
        _userService
            .EnsureCurrentUserCanAsync(Arg.Any<OperationAuthorizationRequirement>(), recipe)
            .Returns(Task.FromException(new UnauthorizedAccessException()));

        var act = async () => await _sut.UpdateRecipeAsync(recipe.Id, request);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
        await _recipeRepository.DidNotReceive().SaveAsync();
    }

    // ── DeleteCommentByIdAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task DeleteCommentByIdAsync_ChecksBothAuthorizationsAndRemovesComment()
    {
        var user = new User { Id = Guid.NewGuid(), Image = null };
        var comment = new Comment { Id = Guid.NewGuid(), Content = "Nice!", User = user };
        var recipe = NewRecipe();
        recipe.Comments.Add(comment);
        _recipeRepository.GetRecipeByIdAsync(recipe.Id).Returns(recipe);

        await _sut.DeleteCommentByIdAsync(recipe.Id, comment.Id);

        await _userService.Received(1)
            .EnsureCurrentUserCanAsync(
                Arg.Is<OperationAuthorizationRequirement>(r => r.Name == Operations.Update.Name),
                recipe);
        await _userService.Received(1)
            .EnsureCurrentUserCanAsync(
                Arg.Is<OperationAuthorizationRequirement>(r => r.Name == Operations.Delete.Name),
                comment);
        recipe.Comments.Should().NotContain(comment);
        await _recipeRepository.Received(1).SaveAsync();
    }
}
