using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Utils;

namespace Wemcy.RecipeApp.Backend.Services;

public interface IRecipeService
{
    Task AddCommentAsync(Guid recipeId, string content);
    Task<Recipe> CreateRecipeAsync(Recipe recipe);
    Task DeleteCommentByIdAsync(Guid recipeId, Guid commentId);
    Task DeleteRecipeByIdAsync(Guid recipeId);
    Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options);
    Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options);
    Task<ImageResult> GetImageByIdAsync(Guid recipeId, Api.Models.ImageSize imageSize);
    Task<List<Guid>> GetRandomRecipesGuids(int count);
    Task<Recipe> GetRecipeByIdAsync(Guid recipeId);
    Task<IList<Recipe>> GetRecipesByIdsAsync(Guid[] recipeIds);
    Task<IList<T>> ListResipesAsAsync<T>(IQueryFilter<Recipe> recipeFilter);
    Task<PaginatedResult<T>> ListResipesAsAsync<T>(PaginationOptions options, IQueryFilter<Recipe> recipeFilter);
    Task RateRecipeAsync(Guid recipeId, int rating);
    IAsyncEnumerable<T> SearchRecipesByTitleAsAsync<T>(RecipeSearch recipeSearch, RecipeFilter recipeFilter);
    Task UpdateImageByIdAsync(Guid recipeId, Stream imageStream, string name);
    Task<Recipe> UpdateRecipeAsync(Guid recipeId, Api.Models.CreateRecipeRequest createRecipeRequest);
}
