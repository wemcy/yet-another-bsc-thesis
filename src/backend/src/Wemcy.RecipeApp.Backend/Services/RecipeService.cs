using AutoMapper;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Repository;
using Wemcy.RecipeApp.Backend.Security;
using Comment = Wemcy.RecipeApp.Backend.Model.Comment;

namespace Wemcy.RecipeApp.Backend.Services;

public class RecipeService(RecipeRepository recipeRepository, ImageService imageService, IMapper mapper, UserService userService)
{
    private readonly RecipeRepository recipeRepository = recipeRepository;
    private readonly ImageService imageService = imageService;

    public async Task<Recipe> CreateRecipeAsync(Recipe recipe)
    {
        recipe.User = await userService.GetCurrentUserEntityAsync();
        await userService.EnsureAuthorizedAsync(recipe, Operations.Create);
        return await this.recipeRepository.CreateRecipeAsync(recipe);
    }

    public async Task<PaginatedResult<T>> ListResipesAs<T>(PaginationOptions options)
    {
        return await recipeRepository.ListRecipesAs<T>(options);
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

    public async Task UpdateImageByIdAsync(Guid id, Stream imageStream, string name)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Update);
        var image = await imageService.CreateImage(imageStream, name);
        recipe.Image = image;
        await this.recipeRepository.SaveAsync();
        
    }

    public async Task<Stream> GetImageByIdAsync(Guid id)
    {
        var image = await this.recipeRepository.GetImageByIdAsync(id);
        return imageService.GetImageById(image.Id);

    }

    public async Task RateRecipeAsync(Guid id, int rating)
    {
        var currentUser = await userService.GetCurrentUserEntityAsync();
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        recipe.Rate(rating, currentUser);
        await this.recipeRepository.SaveAsync();
    }

    public async Task AddCommentAsync(Guid id, string content)
    {
        var currentUser = await userService.GetCurrentUserEntityAsync();
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        recipe.Comments.Add(new Comment() { Content = content, User = currentUser });
        await this.recipeRepository.SaveAsync();
    }

    public async Task DeleteRecipeByIdAsync(Guid id)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Delete);
        this.recipeRepository.DeleteRecipe(recipe);
        await this.recipeRepository.SaveAsync();
    }
    
    public async Task<Recipe> UpdateRecipeAsync(Guid id, Api.Models.CreateRecipeDTO createRecipeDTO)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        await userService.EnsureAuthorizedAsync(recipe, Operations.Update);
        mapper.Map(createRecipeDTO, recipe);
        await this.recipeRepository.SaveAsync();
        return recipe;
    }

    internal async Task<PaginatedResult<T>> GetAllRecipeByAuthorIdAs<T>(Guid id, PaginationOptions options)
    {
        return await this.recipeRepository.GetAllRecipeByAuthorIdAs<T>(id, options);
    }

    internal async Task<PaginatedResult<T>> GetCommentsByRecipeIdAs<T>(Guid id, PaginationOptions options)
    {
        return await this.recipeRepository.GetCommentsByRecipeIdAs<T>(id, options);
    }

    internal async Task<IEnumerable<Comment>> GetCommentsByRecipeId(Guid id)
    {
        var recipe = await this.recipeRepository.GetRecipeByIdAsync(id);
        return recipe.Comments;
    }
}
