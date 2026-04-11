using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Pagination;
using Wemcy.RecipeApp.Backend.Search;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;
using Wemcy.RecipeApp.Backend.Utils;
using Comment = Wemcy.RecipeApp.Backend.Api.Models.Comment;

namespace Wemcy.RecipeApp.Backend.Controllers;

[EntityNotFoundHandler]
[UnauthorizedHandler]
public class RecipeController(RecipeService recipeService, IMapper mapper, ShowcaseRecipeService showcaseRecipeService) : RecipesApiController
{
    public override async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var recipe = mapper.Map<Recipe>(createRecipeDTO);
        await recipeService.CreateRecipeAsync(recipe);
        return Ok(mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> GetRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        var recipe = await recipeService.GetRecipeByIdAsync(id);
        return Ok(mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> GetFeaturedRecipe()
    {
        var recipe = await showcaseRecipeService.GetFeaturedRecipeAsync();
        return Ok(mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> ListShowcaseRecipes()
    {
        var dtos = await showcaseRecipeService.GetShowcaseRecipes<ReadRecipeDTO>();
        return Ok(dtos);
    }

    public override async Task<IActionResult> UpdateRecipeImage([FromRoute(Name = "id"), Required] Guid id, IFormFile image)
    {
        await recipeService.UpdateImageByIdAsync(id, image.OpenReadStream(), image.Name);
        return NoContent();
    }

    public override async Task<IActionResult> GetRecipeImage([FromRoute(Name = "id"), Required] Guid id)
    {
        try
        {
            return new FileStreamResult(await recipeService.GetImageByIdAsync(id), "image/jpeg");
        }
        catch (ImageNotFoundException)
        {
            return File(DefaultImages.DefaultImageSvg, "image/svg+xml");
        }

    }

    public override async Task<IActionResult> RateRecipe([FromRoute(Name = "id"), Required] Guid id, [FromBody] RateRecipeRequest rateRecipeRequest)
    {
        await recipeService.RateRecipeAsync(id, rateRecipeRequest.Rating);
        return NoContent();
    }

    public async override Task<IActionResult> AddRecipeComment([FromRoute(Name = "id"), Required] Guid id, [FromBody] AddRecipeCommentRequest addRecipeCommentRequest)
    {
        await recipeService.AddCommentAsync(id, addRecipeCommentRequest.Content);
        return NoContent();
    }

    public override async Task<IActionResult> DeleteRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        await recipeService.DeleteRecipeByIdAsync(id);
        return NoContent();
    }

    public override async Task<IActionResult> UpdateRecipeById([FromRoute(Name = "id"), Required] Guid id, [FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var recipe = await recipeService.UpdateRecipeAsync(id, createRecipeDTO);
        return Ok(mapper.Map<ReadRecipeDTO>(recipe));

    }

    public override async Task<IActionResult> GetRecipesByAuthorId([FromRoute(Name = "id"), Required] Guid id, [FromQuery(Name = "page"), Range(0, int.MaxValue)] int? page, [FromQuery(Name = "pageSize"), Range(25, 100)] int? pageSize)
    {
        var dtos = await recipeService.GetAllRecipeByAuthorIdAs<ReadRecipeDTO>(id, new PaginationOptions(page, pageSize));
        return Ok(dtos);
    }

    public override async Task<IActionResult> ListRecipeComments([FromRoute(Name = "id"), Required] Guid id, [FromQuery(Name = "page"), Range(0, int.MaxValue)] int? page, [FromQuery(Name = "pageSize"), Range(25, 100)] int? pageSize)
    {
        var dtos = await recipeService.GetCommentsByRecipeIdAs<Comment>(id, new PaginationOptions(page, pageSize));
        return Ok(dtos);
    }

    public override async Task<IActionResult> DeleteRecipeComment([FromRoute(Name = "recipeId"), Required] Guid recipeId, [FromRoute(Name = "commentId"), Required] Guid commentId)
    {
        await recipeService.DeleteCommentByIdAsync(recipeId, commentId);
        return NoContent();
    }

    public override async Task<IActionResult> SearchRecipes([FromQuery(Name = "title"), Required] string title, [FromQuery(Name = "includeAllergens")] List<Allergen>? includeAllergens, [FromQuery(Name = "excludeAllergens")] List<Allergen>? excludeAllergens)
    {
        var includeAllergenTypes = includeAllergens?.Any() ?? false ? mapper.Map<AllergenType?>(includeAllergens) : null;
        var excludeAllergenTypes = excludeAllergens?.Any() ?? false ? mapper.Map<AllergenType?>(excludeAllergens) : null;
        var recipes = recipeService.SearchRecipesByTitleAs<SearchRecipeDTO>(new RecipeSearch(title), new RecipeFilter(includeAllergenTypes, excludeAllergenTypes));
        return Ok(recipes);
    }

    public override async Task<IActionResult> ListRecipes([FromQuery(Name = "page")] int? page, [FromQuery(Name = "pageSize"), Range(25, 100)] int? pageSize, [FromQuery(Name = "includeAllergens")] List<Allergen>? includeAllergens, [FromQuery(Name = "excludeAllergens")] List<Allergen>? excludeAllergens)
    {
        var includeAllergenTypes = includeAllergens?.Any() ?? false ? mapper.Map<AllergenType?>(includeAllergens) : null;
        var excludeAllergenTypes = excludeAllergens?.Any() ?? false ? mapper.Map<AllergenType?>(excludeAllergens) : null;

        var dtos = await recipeService.ListResipesAs<ReadRecipeDTO>(new PaginationOptions(page, pageSize), new RecipeFilter(includeAllergenTypes, excludeAllergenTypes));
        return Ok(dtos);
    }

    [Authorize(Roles = Roles.Admin)]
    public override async Task<IActionResult> UpdateFeaturedRecipe([FromBody] UpdateFeaturedRecipeRequest updateFeaturedRecipeRequest)
    {
        await showcaseRecipeService.SetFeaturedRecipe(updateFeaturedRecipeRequest.RecipeId);
        return NoContent();
    }
}
