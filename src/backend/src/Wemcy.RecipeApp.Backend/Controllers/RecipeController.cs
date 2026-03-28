using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Controllers.ErrorHandler;
using Wemcy.RecipeApp.Backend.Database;
using Wemcy.RecipeApp.Backend.Exceptions;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

[RecipeNotFoundHandler]
[ImageNotFoundHandler]
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
}
