using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Repository
{
    public class RecipeRepository
    {
        private DatabaseContext _dbContext;
        public RecipeRepository(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public Recipe SaveRecipe(Recipe recipe)
        {
            var newRecipe = _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();
            return newRecipe.Entity;
        }

        public IEnumerable<Recipe> GetAllRecipe()
        {
            return _dbContext.Recipes.AsNoTracking();
        }
    }
}
