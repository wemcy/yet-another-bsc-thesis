using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Repository
{
    public class RecipeRepository(DatabaseContext databaseContext)
    {
        private readonly DatabaseContext _dbContext = databaseContext;

        public Recipe SaveRecipe(Recipe recipe)
        {
            foreach (var allergen in recipe.Allergens)
            {
                _dbContext.Attach(allergen);
            }
            var newRecipe = _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();
            return newRecipe.Entity;
        }

        public IQueryable<Recipe> GetAllRecipe()
        {
            var breakpoint = _dbContext.Recipes.AsNoTracking().Include(x => x.Allergens);
            return breakpoint;
        }

        public Recipe? GetRecipeById(Guid id)
        {
            return _dbContext.Recipes.Where(x=> x.Id == id).Include(x => x.Allergens).SingleOrDefault();
        }
    }
}
