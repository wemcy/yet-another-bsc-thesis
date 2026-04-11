using Wemcy.RecipeApp.Backend.Model;

namespace Wemcy.RecipeApp.Backend.Repository
{
    public interface IRecipeShowcaseRepository
    {
        Task<Recipe> GetFeaturedRecipe();
        Task<RecipeShowcase> GetRecipeShowcase();
        Task<IList<Guid>> GetShowcaseRecipeIds();
        Task SaveAsync();
    }
}