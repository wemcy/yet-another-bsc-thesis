using Microsoft.Extensions.Options;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Configuration;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Extensions;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;
using Wemcy.RecipeApp.Backend.Utils;
using Comment = Wemcy.RecipeApp.Backend.Api.Models.Comment;
using Recipe = Wemcy.RecipeApp.Backend.Model.Entities.Recipe;

namespace Wemcy.RecipeApp.Backend.Controllers;

public class RecipeController(IRecipeService recipeService, IMapper mapper, IShowcaseRecipeService showcaseRecipeService, IOptions<PaginationSettings> paginationSettings) : RecipesApiController
{
    private readonly PaginationSettings _paginationSettings = paginationSettings.Value;
    public override async Task<IActionResult> GetRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        var recipe = await recipeService.GetRecipeByIdAsync(id);
        return Ok(mapper.Map<Api.Models.Recipe>(recipe));
    }

    public override async Task<IActionResult> GetFeaturedRecipe()
    {
        var recipe = await showcaseRecipeService.GetFeaturedRecipeAsync();
        return Ok(mapper.Map<Api.Models.Recipe>(recipe));
    }

    public override async Task<IActionResult> ListShowcaseRecipes()
    {
        var dtos = await showcaseRecipeService.GetShowcaseRecipesAsync<Api.Models.Recipe>();
        return Ok(dtos);
    }

    public override async Task<IActionResult> UpdateRecipeImage([FromRoute(Name = "id"), Required] Guid id, IFormFile image)
    {
        await recipeService.UpdateImageByIdAsync(id, image.OpenReadStream(), image.Name);
        return NoContent();
    }

    public override async Task<IActionResult> RateRecipe([FromRoute(Name = "id"), Required] Guid id, [FromBody] Api.Models.RateRecipeRequest rateRecipeRequest)
    {
        await recipeService.RateRecipeAsync(id, rateRecipeRequest.Rating);
        return NoContent();
    }

    public async override Task<IActionResult> AddRecipeComment([FromRoute(Name = "id"), Required] Guid id, [FromBody] Api.Models.AddRecipeCommentRequest addRecipeCommentRequest)
    {
        await recipeService.AddCommentAsync(id, addRecipeCommentRequest.Content);
        return NoContent();
    }

    public override async Task<IActionResult> DeleteRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        await recipeService.DeleteRecipeByIdAsync(id);
        return NoContent();
    }

    public override async Task<IActionResult> UpdateRecipeById([FromRoute(Name = "id"), Required] Guid id, [FromBody] Api.Models.CreateRecipeRequest createRecipeRequest)
    {
        var recipe = await recipeService.UpdateRecipeAsync(id, createRecipeRequest);
        return Ok(mapper.Map<Api.Models.Recipe>(recipe));

    }

    public override async Task<IActionResult> GetRecipesByAuthorId([FromRoute(Name = "id"), Required] Guid id, [FromQuery(Name = "page"), Range(0, int.MaxValue)] int? page, [FromQuery(Name = "pageSize"), Range(25, 100)] int? pageSize)
    {
        var dtos = await recipeService.GetAllRecipeByAuthorIdAs<Api.Models.Recipe>(id, new PaginationOptions(page ?? 0, pageSize ?? _paginationSettings.DefaultPageSize));
        return Ok(dtos);
    }

    public override async Task<IActionResult> ListRecipeComments([FromRoute(Name = "id"), Required] Guid id, [FromQuery(Name = "page"), Range(0, int.MaxValue)] int? page, [FromQuery(Name = "pageSize"), Range(25, 100)] int? pageSize)
    {
        var dtos = await recipeService.GetCommentsByRecipeIdAs<Comment>(id, new PaginationOptions(page ?? 0, pageSize ?? _paginationSettings.DefaultPageSize));
        return Ok(dtos);
    }

    public override async Task<IActionResult> DeleteRecipeComment([FromRoute(Name = "recipeId"), Required] Guid recipeId, [FromRoute(Name = "commentId"), Required] Guid commentId)
    {
        await recipeService.DeleteCommentByIdAsync(recipeId, commentId);
        return NoContent();
    }

    public override async Task<IActionResult> SearchRecipes([FromQuery(Name = "title"), Required] string title, [FromQuery(Name = "includeAllergens")] List<Api.Models.Allergen>? includeAllergens, [FromQuery(Name = "excludeAllergens")] List<Api.Models.Allergen>? excludeAllergens)
    {
        var includeAllergenTypes = includeAllergens.ApplyMapIfHasElementOrDefault(mapper.Map<AllergenType?>);
        var excludeAllergenTypes = excludeAllergens.ApplyMapIfHasElementOrDefault(mapper.Map<AllergenType?>);
        var recipes = recipeService.SearchRecipesByTitleAsAsync<Api.Models.RecipeSummary>(new RecipeSearch(title), new RecipeFilter(includeAllergenTypes, excludeAllergenTypes));
        return Ok(recipes);
    }

    public override async Task<IActionResult> ListRecipes([FromQuery(Name = "page")] int? page, [FromQuery(Name = "pageSize"), Range(25, 100)] int? pageSize, [FromQuery(Name = "includeAllergens")] List<Api.Models.Allergen>? includeAllergens, [FromQuery(Name = "excludeAllergens")] List<Api.Models.Allergen>? excludeAllergens)
    {
        var includeAllergenTypes = includeAllergens.ApplyMapIfHasElementOrDefault(mapper.Map<AllergenType?>);
        var excludeAllergenTypes = excludeAllergens.ApplyMapIfHasElementOrDefault(mapper.Map<AllergenType?>);

        var dtos = await recipeService.ListResipesAsAsync<Api.Models.Recipe>(new PaginationOptions(page ?? 0, pageSize ?? _paginationSettings.DefaultPageSize), new RecipeFilter(includeAllergenTypes, excludeAllergenTypes));
        return Ok(dtos);
    }

    [Authorize(Roles = Roles.Admin)]
    public override async Task<IActionResult> UpdateFeaturedRecipe([FromBody] Api.Models.UpdateFeaturedRecipeRequest updateFeaturedRecipeRequest)
    {
        await showcaseRecipeService.SetFeaturedRecipeAsync(updateFeaturedRecipeRequest.RecipeId);
        return NoContent();
    }

    public override async Task<IActionResult> CreateRecipe([FromBody] Api.Models.CreateRecipeRequest createRecipeRequest)
    {
        var recipe = mapper.Map<Recipe>(createRecipeRequest);
        await recipeService.CreateRecipeAsync(recipe);
        return Ok(mapper.Map<Api.Models.Recipe>(recipe));
    }

    public  override async Task<IActionResult> GetRecipeImage([FromRoute(Name = "id"), Required] Guid id, [FromQuery(Name = "size")] Api.Models.ImageSize? size)
    {
        try
        {
            return new FileStreamResult(await recipeService.GetImageByIdAsync(id, size ?? ImageSize.LargeEnum), "image/jpeg");
        }
        catch (ImageNotFoundException)
        {
            return File(DefaultImages.DefaultImageSvg, "image/svg+xml");
        }
    }
}
