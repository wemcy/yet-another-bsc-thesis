using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Repository
{
    public class RecipeRepository(DatabaseContext databaseContext)
    {
        private readonly DatabaseContext _dbContext = databaseContext;

        public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
        {
            foreach (var allergen in recipe.Allergens)
            {
                _dbContext.Attach(allergen);
            }
            var newRecipe = _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();
            return newRecipe.Entity;
        }

        public async Task<IList<Recipe>> GetAllRecipeAsync()
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .Include(x => x.Allergens)
                .Include( x => x.Comments)
                .ToListAsync();
        }

        public async Task<IList<Recipe>> GetRecipesAsync(int limit)
        {
            return await _dbContext.Recipes
                .AsNoTracking()
                .Include(x => x.Allergens)
                .Include(x => x.Comments)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeByIdWithAllergensAsync(Guid id)
        {
            return await _dbContext.Recipes
                .Where(x => x.Id == id)
                .Include(x => x.Allergens)
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
    }
}
