using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Repository;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService(RecipeRepository recipeRepository, ImageService imageService)
{
    private readonly RecipeRepository recipeRepository = recipeRepository;
    private readonly ImageService imageService = imageService;

    public Recipe CreateRecipe(Recipe recipe)
    {
        return this.recipeRepository.CreateRecipe(recipe);
    }

    public IQueryable<Recipe> GetAllRecipe()
    {
        return this.recipeRepository.GetAllRecipe();
    }

    public Recipe? GetRecipeById(Guid id)
    {
        return this.recipeRepository.GetRecipeByIdWithAllergens(id);
    }

    public IQueryable<Recipe> GetShowcaseRecieps()
    {
        return this.recipeRepository.GetAllRecipe().Take(6);
    }

    public Recipe? GetFeaturedRecipe()
    {
        // TODO: implement admin selected featured recipe
        return this.recipeRepository.GetAllRecipe().FirstOrDefault();
    }

    public async Task UpdageImageById(Guid id, Stream imageStream, string name)
    {
        var recipe = this.recipeRepository.GetRecipeById(id) ?? throw new KeyNotFoundException($"Recipe with id {id} not found");
        var image = await imageService.CreateImage(imageStream, name);
        recipe.Image = image;
        this.recipeRepository.SaveRecipe(recipe);
    }

    public Stream? GetImageById(Guid id)
    {
        var image = this.recipeRepository.GetImageById(id) ?? throw new KeyNotFoundException($"Recipe with id {id} not found");
        if (image == null) return null;
        return  imageService.GetImageById(image.Id);
    }

    public bool RateRecipe(Guid id, int rating)
    {
        if(this.recipeRepository.GetRecipeById(id, out var recipe))
        {
            recipe.Rate(rating);
            this.recipeRepository.Save();
            return true;
        }
        return false;
    }
}
