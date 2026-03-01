using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
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
            var breakpoint = _dbContext.Recipes.AsNoTracking().Include(x => x.Allergens).Include( x => x.Comments);
            return breakpoint;
        }

        public Recipe GetRecipeByIdWithAllergens(Guid id)
        {
            return _dbContext.Recipes.Where(x => x.Id == id).Include(x => x.Allergens).SingleOrDefault() ?? throw new RecipeNotFoundException(id);
        }

        public Recipe GetRecipeById(Guid id)
        {
            return _dbContext.Recipes.Where(x => x.Id == id).SingleOrDefault() ?? throw new RecipeNotFoundException(id);
        }

        public Image GetImageById(Guid id)
        {
            return GetRecipeById(id).Image ?? throw new ImageNotFoundException();
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
