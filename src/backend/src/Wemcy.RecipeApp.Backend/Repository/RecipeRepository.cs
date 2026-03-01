using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Repository
{
    public class RecipeRepository(DatabaseContext databaseContext)
    {
        private readonly DatabaseContext _dbContext = databaseContext;

        public Recipe CreateRecipe(Recipe recipe)
        {
            foreach (var allergen in recipe.Allergens)
            {
                _dbContext.Attach(allergen);
            }
            var newRecipe = _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();
            return newRecipe.Entity;
        }

        public Recipe SaveRecipe(Recipe recipe)
        {
            var updatedRecipe = _dbContext.Recipes.Update(recipe);
            _dbContext.SaveChanges();
            return updatedRecipe.Entity;
        }

        public IQueryable<Recipe> GetAllRecipe()
        {
            var breakpoint = _dbContext.Recipes.AsNoTracking().Include(x => x.Allergens);
            return breakpoint;
        }

        public Recipe? GetRecipeByIdWithAllergens(Guid id)
        {
            return _dbContext.Recipes.Where(x => x.Id == id).Include(x => x.Allergens).SingleOrDefault();
        }
        public Recipe? GetRecipeById(Guid id)
        {
            return _dbContext.Recipes.Where(x => x.Id == id).SingleOrDefault();
        }

        public Image GetImageById(Guid id)
        {
            var recipe = _dbContext.Recipes.Where(x => x.Id == id).Include(x => x.Image).SingleOrDefault() ?? throw new KeyNotFoundException($"Recipe with id {id} not found");
            if (recipe.Image == null) throw new KeyNotFoundException($"Image for recipe with id {id} not found");
            return recipe.Image;

        }
    }
}
