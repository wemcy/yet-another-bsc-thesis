using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;
[RecipeNotFoundHandler]

public class RecipeController(RecipeService recipeService, IMapper mapper) : RecipesApiController
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
        var recipe = await recipeService.GetFeaturedRecipeAsync();
        return Ok(mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> ListRecipes()
    {
        var q = await recipeService.GetAllRecipe();
        var dtos = q.Select(x => mapper.Map<ReadRecipeDTO>(x)).ToList();
        return Ok(dtos);
    }

    public override async Task<IActionResult> ListShowcaseRecipes()
    {
        var q = await recipeService.GetShowcaseRecieps();
        var dtos = q.Select(x => mapper.Map<ReadRecipeDTO>(x)).ToList();
        return Ok(dtos);
    }

    public override async Task<IActionResult> UpdateRecipeImage([FromRoute(Name = "id"), Required] Guid id, IFormFile image)
    {
        // TODO 404
        await recipeService.UpdageImageById(id, image.OpenReadStream(), image.Name);
        return NoContent();
    }
    [ImageNotFoundHandler]
    public override async Task<IActionResult> GetRecipeImage([FromRoute(Name = "id"), Required] Guid id)
    {
        return new FileStreamResult( await recipeService.GetImageById(id),"image/jpeg");
    }

    public override async Task<IActionResult> RateRecipe([FromRoute(Name = "id"), Required] Guid id, [FromBody] RateRecipeRequest rateRecipeRequest)
    {
        await recipeService.RateRecipe(id, rateRecipeRequest.Rating);
        return NoContent();
    }

    public async override Task<IActionResult> AddRecipeComment([FromRoute(Name = "id"), Required] Guid id, [FromBody] AddRecipeCommentRequest addRecipeCommentRequest)
    {
        await recipeService.AddComment(id,addRecipeCommentRequest.Content);
        return NoContent();
    }

    public override Task<IActionResult> GetRecipeComments([FromRoute(Name = "id"), Required] Guid id)
    {
        throw new NotImplementedException();
    }

    public override Task<IActionResult> DeleteRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        throw new NotImplementedException();
    }
}
