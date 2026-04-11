using Wemcy.RecipeApp.Backend.Model.Entities;

namespace Wemcy.RecipeApp.Backend.Services
{
    public interface IShowcaseRecipeService
    {
        Task CreateDeafaultShowcaseAndFeaturedRecipeAsync();
        Task<Recipe> GetFeaturedRecipeAsync();
        Task<IList<T>> GetShowcaseRecipesAsync<T>();
        Task SetFeaturedRecipeAsync(Guid featuredRecipeId);
        Task UpdateShowcaseRecipesAsync();
    }
}
