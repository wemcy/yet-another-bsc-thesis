using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Security;
using Comment = Wemcy.RecipeApp.Backend.Model.Comment;
using Recipe = Wemcy.RecipeApp.Backend.Model.Recipe;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService(IRecipeRepository recipeRepository, ImageService imageService, IMapper mapper, IUserService userService)
{
    private readonly IRecipeRepository recipeRepository = recipeRepository;
    private readonly ImageService imageService = imageService;

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        recipe.User = await userService.GetCurrentUserEntityAsync();
        await userService.EnsureAuthorizedAsync(recipe, Operations.Create);
        return await this.recipeRepository.CreateRecipeAsync(recipe);
    }

    public async Task<PaginatedResult<T>> ListResipesAs<T>(PaginationOptions options, IQueryFilter<Recipe> recipeFilter)
    {
        return await recipeRepository.ListRecipesAs<T>(options, recipeFilter);
    }

    public async Task<IList<T>> ListResipesAs<T>(IQueryFilter<Recipe> recipeFilter)
    {
        return await recipeRepository.ListRecipesAs<T>(recipeFilter);
    }

    public async Task<Recipe> GetRecipeByIdAsync(Guid id)
    {
        return await this.recipeRepository.GetRecipeByIdAsync(id);
    }

    public async Task UpdateImageByIdAsync(Guid id, Stream imageStream, string name)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Update);
        var image = await imageService.CreateImage(imageStream, name);
        recipe.Image = image;
        await this.recipeRepository.SaveAsync();

    }

    public async Task<Stream> GetImageByIdAsync(Guid id)
    {
        var image = await this.recipeRepository.GetImageByIdAsync(id);
        return imageService.GetImageById(image.Id);

    }

    public async Task RateRecipeAsync(Guid id, int rating)
    {
        var currentUser = await userService.GetCurrentUserEntityAsync();
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        recipe.Rate(rating, currentUser);
        await this.recipeRepository.SaveAsync();
    }

    public async Task AddCommentAsync(Guid id, string content)
    {
        var currentUser = await userService.GetCurrentUserEntityAsync();
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        recipe.Comments.Add(new Comment() { Content = content, User = currentUser });
        await this.recipeRepository.SaveAsync();
    }

    public async Task DeleteRecipeByIdAsync(Guid id)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Delete);
        this.recipeRepository.DeleteRecipe(recipe);
        await this.recipeRepository.SaveAsync();
    }

    public async Task<Recipe> UpdateRecipeAsync(Guid id, CreateRecipeRequest createRecipeRequest)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Update);
        mapper.Map(createRecipeRequest, recipe);
        await this.recipeRepository.SaveAsync();
        return recipe;
    }

    public async Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options)
    {
        return await this.recipeRepository.GetAllRecipeByAuthorIdAs<T>(id, options);
    }

    public async Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options)
    {
        return await this.recipeRepository.GetCommentsByRecipeIdAs<T>(id, options);
    }


    public async Task DeleteCommentByIdAsync(Guid recipeId, Guid commentId)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(recipeId);
        var comment = recipe.GetCommentById(commentId);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Update);
        await userService.EnsureAuthorizedAsync(comment, Operations.Delete);
        recipe.Comments.Remove(comment);
        await this.recipeRepository.SaveAsync();
    }

    public IAsyncEnumerable<T> SearchRecipesByTitleAs<T>(RecipeSearch recipeSearch, RecipeFilter recipeFilter)
    {
        return this.recipeRepository.SearchRecipesByTitleAs<T>(recipeSearch, recipeFilter);
    }

    public async Task<List<Guid>> GetRandomRecipesGuids(int count)
    {
        return await this.recipeRepository.GetRandomRecipesGuids(count);
    }

    public async Task<IList<Recipe>> GetRecipesByIdsAsync(Guid[] ids)
    {
        return await this.recipeRepository.GetRecipesByIdsAsync(ids);
    }
}
