using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Repository;

public interface IRecipeRepository
{
    Task<Recipe> CreateRecipeAsync(Recipe recipe);
    Task DeleteCommentById(Guid recipeId, Guid commentId);
    void DeleteRecipe(Recipe recipe);
    Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options);
    Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options);
    Task<Image> GetImageByIdAsync(Guid id);
    Task<List<Guid>> GetRandomRecipesGuids(int count);
    Task<Recipe> GetRecipeByIdAsync(Guid id);
    Task<IList<Recipe>> GetRecipesByIdsAsync(Guid[] ids);
    Task<IList<T>> ListRecipesAs<T>(IQueryFilter<Recipe> recipeFilter);
    Task<PaginatedResult<T>> ListRecipesAs<T>(PaginationOptions options, IQueryFilter<Recipe> recipeFilter);
    Task SaveAsync();
    IAsyncEnumerable<T> SearchRecipesByTitleAs<T>(RecipeSearch recipeSearch, RecipeFilter recipeFilter);
}
