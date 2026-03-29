using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Pagination;

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

    public async Task<IList<Recipe>> GetAllRecipeByAuthorIdAsync(Guid id)
    {
        return await _dbContext.Recipes.Where(x => x.User.Id == id).AsNoTracking().Include(x => x.User).Include(x => x.Comments).ToListAsync();
    }

    public async Task<PaginatedResult<T>> ListRecipesAs<T>(PaginationOptions options)
    {
        return await _dbContext.Recipes.AsNoTracking().ProjectTo<T>(mapper.ConfigurationProvider).ToPaginatedListAsync(options);
    }
}
