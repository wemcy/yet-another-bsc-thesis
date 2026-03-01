using Wemcy.RecipeApp.Backend.Exceptions;
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

    public Recipe GetRecipeById(Guid id)
    {
        return this.recipeRepository.GetRecipeByIdWithAllergens(id) ?? throw new RecipeNotFoundException(id);
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
        var recipe = this.recipeRepository.GetRecipeById(id);
        var image = await imageService.CreateImage(imageStream, name);
        recipe.Image = image;
        this.recipeRepository.Save();
        
    }

    public Stream GetImageById(Guid id)
    {
        var image = this.recipeRepository.GetImageById(id);
        return imageService.GetImageById(image.Id);

    }

    public void RateRecipe(Guid id, int rating)
    {
        var recipe = this.recipeRepository.GetRecipeById(id);
        recipe.Rate(rating);
        this.recipeRepository.Save();
    }

    public void AddComment(Guid id, string content)
    {
        var recipe = this.recipeRepository.GetRecipeById(id);
        recipe.Comments.Add(new Comment() { Content = content });
        this.recipeRepository.Save();
    }
}
