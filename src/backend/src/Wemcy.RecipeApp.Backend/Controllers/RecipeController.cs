using Microsoft.AspNetCore.Mvc;
using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Model;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers;

public class RecipeController : RecipesApiController
{
    private readonly RecipeService _recipeService;

    public RecipeController(RecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    public override IActionResult CreateRecipe([FromBody] CreateRecipeDTO createRecipeDTO)
    {
        var recipe = new Recipe() {
            Id = Guid.NewGuid(),
            Title = createRecipeDTO.Title,
            Description = createRecipeDTO.Description
        };
        _recipeService.SaveRecipe(recipe);
        return Ok(new ReadRecipeDTO()
        {
            CreatedAt = DateTime.Now,
            Id = recipe.Id,
            Description = recipe.Description,
            Title = recipe.Title,
            UpdatedAt = DateTime.Now,
        });
    }

    public override IActionResult ListRecipes()
    {
        var recipes = _recipeService.GetAllRecipe();
        return Ok(recipes.Select( rec => new ReadRecipeDTO
        {
            Id = rec.Id,
            Title = rec.Title,
            Description = rec.Description,
            UpdatedAt = DateTime.Now,
            CreatedAt = DateTime.Now,
        }).ToList());
    }
}
