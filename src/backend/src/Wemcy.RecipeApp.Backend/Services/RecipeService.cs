using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService(RecipeRepository recipeRepository, ImageService imageService)
{
    private readonly RecipeRepository recipeRepository = recipeRepository;
    private readonly ImageService imageService = imageService;

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        return await this.recipeRepository.CreateRecipeAsync(recipe);
    }

    public async Task<IList<Recipe>> GetAllRecipe()
    {
        return await this.recipeRepository.GetAllRecipeAsync();
    }

    public async Task<Recipe> GetRecipeByIdAsync(Guid id)
    {
        return await this.recipeRepository.GetRecipeByIdWithAllergensAsync(id);
    }

    public async Task<IList<Recipe>> GetShowcaseRecieps()
    {
        return await this.recipeRepository.GetRecipesAsync(6);
    }

    public async Task<Recipe?> GetFeaturedRecipeAsync()
    {
        // TODO: implement admin selected featured recipe
        return (await this.recipeRepository.GetRecipesAsync(1)).FirstOrDefault();
    }

    public async Task UpdageImageById(Guid id, Stream imageStream, string name)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        var image = await imageService.CreateImage(imageStream, name);
        recipe.Image = image;
        await this.recipeRepository.SaveAsync();
        
    }

    public async Task<Stream> GetImageById(Guid id)
    {
        var image = await this.recipeRepository.GetImageByIdAsync(id);
        return imageService.GetImageById(image.Id);

    }

    public async Task RateRecipe(Guid id, int rating)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        recipe.Rate(rating);
        await this.recipeRepository.SaveAsync();
    }

    public async Task AddComment(Guid id, string content)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        recipe.Comments.Add(new Comment() { Content = content });
        await this.recipeRepository.SaveAsync();
    }
}
