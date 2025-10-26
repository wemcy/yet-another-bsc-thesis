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

        public IEnumerable<Recipe> GetAllRecipe()
        {
            return _dbContext.Recipes.AsNoTracking().Include(x => x.Allergens);
        }

        public Recipe? GetRecipeById(Guid id)
        {
            return _dbContext.Recipes.Find(id);
        }
    }
}
