using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

public class RecipeController : RecipesApiController
{
    private readonly RecipeService _recipeService;
    private readonly IMapper _mapper;

    public RecipeController(RecipeService recipeService, IMapper mapper)
    {
        _recipeService = recipeService;
        _mapper = mapper;
    }

    public override IActionResult CreateRecipe([FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var recipe = _mapper.Map<Recipe>(createRecipeDTO);
        _recipeService.CreateRecipe(recipe);
        return Ok(_mapper.Map<ReadRecipeDTO>(recipe));
    }

    public override IActionResult ListRecipes()
    {
        return Ok(_mapper.ProjectTo<ReadRecipeDTO>(_recipeService.GetAllRecipe().AsQueryable()));
    }
}
