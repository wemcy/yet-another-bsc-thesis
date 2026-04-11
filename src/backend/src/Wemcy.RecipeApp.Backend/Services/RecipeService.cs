using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Security;
using Comment = Wemcy.RecipeApp.Backend.Model.Entities.Comment;
using Recipe = Wemcy.RecipeApp.Backend.Model.Entities.Recipe;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService(IRecipeRepository recipeRepository, ImageService imageService, IMapper mapper, IUserService userService) : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly ImageService _imageService = imageService;
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        recipe.User = await _userService.GetCurrentUserAsync();
        await _userService.EnsureCurrentUserCanAsync(Operations.Create, recipe);
        return await this._recipeRepository.CreateRecipeAsync(recipe);
    }

    public async Task<PaginatedResult<T>> ListResipesAsAsync<T>(PaginationOptions options, IQueryFilter<Recipe> recipeFilter)
    {
        return await _recipeRepository.ListRecipesAs<T>(options, recipeFilter);
    }

    public async Task<IList<T>> ListResipesAsAsync<T>(IQueryFilter<Recipe> recipeFilter)
    {
        return await _recipeRepository.ListRecipesAs<T>(recipeFilter);
    }

    public async Task<Recipe> GetRecipeByIdAsync(Guid id)
    {
        return await this._recipeRepository.GetRecipeByIdAsync(id);
    }

    public async Task UpdateImageByIdAsync(Guid id, Stream imageStream, string name)
    {
        var recipe = await this._recipeRepository.GetRecipeByIdAsync(id);
        await _userService.EnsureCurrentUserCanAsync(Operations.Update, recipe);
        var image = await _imageService.CreateImage(imageStream, name);
        recipe.Image = image;
        await this._recipeRepository.SaveAsync();

    }

    public async Task<Stream> GetImageByIdAsync(Guid id)
    {
        var image = await this._recipeRepository.GetImageByIdAsync(id);
        return _imageService.GetImageById(image.Id);

    }

    public async Task RateRecipeAsync(Guid id, int rating)
    {
        var currentUser = await _userService.GetCurrentUserAsync();
        var recipe = await this._recipeRepository.GetRecipeByIdAsync(id);
        recipe.Rate(rating, currentUser);
        await this._recipeRepository.SaveAsync();
    }

    public async Task AddCommentAsync(Guid id, string content)
    {
        var currentUser = await _userService.GetCurrentUserAsync();
        var recipe = await this._recipeRepository.GetRecipeByIdAsync(id);
        recipe.Comments.Add(new Comment() { Content = content, User = currentUser });
        await this._recipeRepository.SaveAsync();
    }

    public async Task DeleteRecipeByIdAsync(Guid id)
    {
        var recipe = await this._recipeRepository.GetRecipeByIdAsync(id);
        await _userService.EnsureCurrentUserCanAsync(Operations.Delete, recipe);
        this._recipeRepository.DeleteRecipe(recipe);
        await this._recipeRepository.SaveAsync();
    }

    public async Task<Recipe> UpdateRecipeAsync(Guid id, CreateRecipeRequest createRecipeRequest)
    {
        var recipe = await this._recipeRepository.GetRecipeByIdAsync(id);
        await _userService.EnsureCurrentUserCanAsync(Operations.Update, recipe);
        _mapper.Map(createRecipeRequest, recipe);
        await this._recipeRepository.SaveAsync();
        return recipe;
    }

    public async Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options)
    {
        return await this._recipeRepository.GetAllRecipeByAuthorIdAs<T>(id, options);
    }

    public async Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options)
    {
        return await this._recipeRepository.GetCommentsByRecipeIdAs<T>(id, options);
    }


    public async Task DeleteCommentByIdAsync(Guid recipeId, Guid commentId)
    {
        var recipe = await this._recipeRepository.GetRecipeByIdAsync(recipeId);
        var comment = recipe.GetCommentById(commentId);
        await _userService.EnsureCurrentUserCanAsync(Operations.Update, recipe);
        await _userService.EnsureCurrentUserCanAsync(Operations.Delete, comment);
        recipe.Comments.Remove(comment);
        await this._recipeRepository.SaveAsync();
    }

    public IAsyncEnumerable<T> SearchRecipesByTitleAsAsync<T>(RecipeSearch recipeSearch, RecipeFilter recipeFilter)
    {
        return this._recipeRepository.SearchRecipesByTitleAs<T>(recipeSearch, recipeFilter);
    }

    public async Task<List<Guid>> GetRandomRecipesGuids(int count)
    {
        return await this._recipeRepository.GetRandomRecipesGuids(count);
    }

    public async Task<IList<Recipe>> GetRecipesByIdsAsync(Guid[] ids)
    {
        return await this._recipeRepository.GetRecipesByIdsAsync(ids);
    }
}
