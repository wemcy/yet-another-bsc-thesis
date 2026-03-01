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

    public override IActionResult CreateRecipe([FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var recipe = _mapper.Map<Recipe>(createRecipeDTO);
        _recipeService.CreateRecipe(recipe);
        return Ok(_mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override IActionResult GetRecipeById([FromRoute(Name = "id"), Required] Guid id)
    {
        var recipe = _recipeService.GetRecipeById(id);
        return recipe == null ? NotFound() : Ok(_mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override IActionResult ListRecipes()
    {
        var q = _recipeService.GetAllRecipe();
        var dtos = q.Select(x => _mapper.Map<ReadRecipeDTO>(x)).ToList();
        return Ok(dtos);
    }

    public override IActionResult ListShowcaseRecipes()
    {
        var q = _recipeService.GetShowcaseRecieps();
        var dtos = q.Select(x => _mapper.Map<ReadRecipeDTO>(x)).ToList();
        return Ok(dtos);
    }
}
