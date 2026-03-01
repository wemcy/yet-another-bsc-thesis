using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

public class RecipeController(RecipeService recipeService, IMapper mapper) : RecipesApiController
{
    private readonly RecipeService _recipeService = recipeService;
    private readonly IMapper _mapper = mapper;

    public override async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var recipe = _mapper.Map<Recipe>(createRecipeDTO);
        _recipeService.CreateRecipe(recipe);
        return Ok(_mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> GetRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        var recipe = _recipeService.GetRecipeById(id);
        return recipe == null ? NotFound() : Ok(_mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> GetFeaturedRecipe()
    {
        var recipe = _recipeService.GetFeaturedRecipe();
        return recipe == null ? NotFound() : Ok(_mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override async Task<IActionResult> ListRecipes()
    {
        var q = _recipeService.GetAllRecipe();
        var dtos = q.Select(x => _mapper.Map<ReadRecipeDTO>(x)).ToList();
        return Ok(dtos);
    }

    public override async Task<IActionResult> ListShowcaseRecipes()
    {
        var q = _recipeService.GetShowcaseRecieps();
        var dtos = q.Select(x => _mapper.Map<ReadRecipeDTO>(x)).ToList();
        return Ok(dtos);
    }

    public override async Task<IActionResult> UpdateRecipeImage([FromRoute(Name = "id"), Required] Guid id, IFormFile image)
    {
        // TODO 404
        await _recipeService.UpdageImageById(id, image.OpenReadStream(), image.Name);
        return NoContent();
    }

    public override async Task<IActionResult> GetRecipeImage([FromRoute(Name = "id"), Required] Guid id)
    {
        return new FileStreamResult( _recipeService.GetImageById(id),"image/jpeg");
    }

    public override async Task<IActionResult> RateRecipe([FromRoute(Name = "id"), Required] Guid id, [FromBody] RateRecipeRequest rateRecipeRequest)
    {
        return _recipeService.RateRecipe(id, rateRecipeRequest.Rating) ? NoContent() : NotFound();
    }
}
