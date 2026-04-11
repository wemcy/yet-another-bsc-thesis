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

public class RecipeRepository(DatabaseContext databaseContext, IMapper mapper) : IRecipeRepository
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    protected DbSet<Recipe> Recipes => _databaseContext.Recipes;
    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        var newRecipe = Recipes.Add(recipe);
        await _databaseContext.SaveChangesAsync();
        return newRecipe.Entity;
    }

    public async Task<Recipe> GetRecipeByIdAsync(Guid id)
    {
        return await Recipes
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync() ?? throw new RecipeNotFoundException(id);
    }

    public async Task<Image> GetImageByIdAsync(Guid id)
    {
        var recipe = await GetRecipeByIdAsync(id);
        return recipe.Image ?? throw new ImageNotFoundException();
    }
    public async Task SaveAsync()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public void DeleteRecipe(Recipe recipe)
    {
        Recipes.Remove(recipe);
    }

    public async Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options)
    {

        return await Recipes
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.User!.Id == id)
            .OrderByDescending(x => x.UpdatedAt)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(options);
    }

    public async Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options)
    {
        var recipe = await GetRecipeByIdAsync(id);
        return await Recipes
            .AsNoTracking()
            .Where(x => x.Id == id)
            .SelectMany(x => x.Comments)
            .OrderByDescending(x => x.UpdatedAt)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(options);
    }

    public async Task<PaginatedResult<T>> ListRecipesAs<T>(PaginationOptions options, IQueryFilter<Recipe> recipeFilter)
    {
        return await Recipes
            .AsNoTracking()
            .WithFilter(recipeFilter)
            .Include(x => x.User)
            .OrderByDescending(x => x.UpdatedAt)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToPaginatedListAsync(options);
    }

    public async Task<IList<T>> ListRecipesAs<T>(IQueryFilter<Recipe> recipeFilter)
    {
        return await Recipes
            .AsNoTracking()
            .WithFilter(recipeFilter)
            .Include(x => x.User)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task DeleteCommentById(Guid recipeId, Guid commentId)
    {
        var recipe = await GetRecipeByIdAsync(recipeId);
        recipe.Comments.Remove(recipe.Comments.FirstOrDefault(c => c.Id == commentId) ?? throw new CommentNotFoundExeption(commentId));
    }

    public IAsyncEnumerable<T> SearchRecipesByTitleAs<T>(RecipeSearch recipeSearch, RecipeFilter recipeFilter)
    {
        return Recipes
            .AsNoTracking()
            .WithFilter(recipeSearch)
            .WithFilter(recipeFilter)
            .Take(10)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToAsyncEnumerable();
    }

    public async Task<List<Guid>> GetRandomRecipesGuids(int count)
    {
        return await Recipes
            .OrderBy(x => EF.Functions.Random())
            .Take(count)
            .Select(x => x.Id)
            .ToListAsync();
    }

    public async Task<IList<Recipe>> GetRecipesByIdsAsync(Guid[] ids)
    {
        return await Recipes
            .AsNoTracking()
            .Where(r => ids.Contains(r.Id))
            .Include(r => r.User)
            .ToListAsync();
    }
}
