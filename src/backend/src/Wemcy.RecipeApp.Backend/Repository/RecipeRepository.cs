using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;

namespace Wemcy.RecipeApp.Backend.Repository;

public class RecipeRepository(DatabaseContext databaseContext, IMapper mapper)
{
    private readonly DatabaseContext _dbContext = databaseContext;

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        var newRecipe = _dbContext.Recipes.Add(recipe);
        await _dbContext.SaveChangesAsync();
        return newRecipe.Entity;
    }

    public async Task<IList<Recipe>> GetAllRecipeAsync()
    {


        return await _dbContext.Recipes
            .AsNoTracking()
            .Include( x => x.Comments)
            .Include( x => x.User)
            .ToListAsync();
    }

    public async Task<IList<Recipe>> GetRecipesAsync(int limit)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(x => x.Comments)
            .Include( x => x.User)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Recipe> GetRecipeByIdWithAllergensAsync(Guid id)
    {
        return await _dbContext.Recipes
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync() ?? throw new RecipeNotFoundException(id);
    }

    public async Task<Recipe> GetRecipeByIdAsync(Guid id)
    {
        return await _dbContext.Recipes
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync() ?? throw new RecipeNotFoundException(id);
    }

    public async Task<Image> GetImageByIdAsync(Guid id)
    {
        return (await GetRecipeByIdAsync(id)).Image ?? throw new ImageNotFoundException();
    }
    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void DeleteRecipe(Recipe recipe)
    {
        _dbContext.Recipes.Remove(recipe);
    }

    public async Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options)
    {

        return await _dbContext.Recipes
            .Include(x => x.User)
            .Where(x => x.User.Id == id)
            .OrderByDescending(x => x.UpdatedAt)
            .AsNoTracking()
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(options);
    }

    public async Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options)
    {
        var recipe = await GetRecipeByIdAsync(id);
        return await _dbContext.Recipes
            .Where(x => x.Id == id)
            .SelectMany(x => x.Comments)
            .OrderByDescending(x => x.UpdatedAt)
            .AsNoTracking()
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(options);
    }

    public async Task<PaginatedResult<T>> ListRecipesAs<T>(PaginationOptions options)
    {
        return await _dbContext.Recipes
            .Include(x=>x.User)
            .OrderByDescending(x => x.UpdatedAt)
            .AsNoTracking()
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(options);
    }

    public async Task DeleteCommentById(Guid recipeId, Guid commentId)
    {
        var recipe = await GetRecipeByIdAsync(recipeId);
        recipe.Comments.Remove(recipe.Comments.FirstOrDefault(c => c.Id == commentId) ?? throw new CommentNotFoundExeption(commentId));
    }

    public  IAsyncEnumerable<T> SearchRecipesByTitleAs<T>(RecipeSearch recipeSearch)
    {
        return _dbContext.Recipes
            .Where(x => x.TitleSearchVector.Matches(recipeSearch.Title))
            .OrderByDescending(x => x.TitleSearchVector.Rank(EF.Functions.PlainToTsQuery(recipeSearch.Title)))
            .ConditionalWhere( () => recipeSearch.HasIncludeFilter, x => (x.Allergens & recipeSearch.IncludeAllergens) != AllergenType.None)
            .ConditionalWhere( () => recipeSearch.HasExcludeFilter, x => (x.Allergens & recipeSearch.ExcludeAllergens) == AllergenType.None)
            .AsNoTracking()
            .Take(10)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToAsyncEnumerable();
    }
}
