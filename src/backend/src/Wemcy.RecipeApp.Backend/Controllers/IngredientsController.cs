using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Api.Models;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers
{
    public class IngredientsController(IngredientSuggestionService ingredientSuggestionService) : IngredientsApiController
    {
        private readonly IngredientSuggestionService _ingredientSuggestionService = ingredientSuggestionService;
        public async override Task<IActionResult> CreateIngredient([FromBody] IngredientSuggestion ingredientSuggestion)
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> DeleteIngredient([FromRoute(Name = "id"), Required] Guid id)
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> GetIngredientById([FromRoute(Name = "id"), Required] Guid id)
        {
            throw new NotImplementedException();
        }

        public async override Task<IActionResult> SearchIngredients([FromQuery(Name = "name"), Required] string name)
        {
            var result = _ingredientSuggestionService.SearchIngredientsAsAsync<Api.Models.IngredientSuggestion>(name);
            return Ok(result);
        }

        public async override Task<IActionResult> UpdateIngredient([FromRoute(Name = "id"), Required] Guid id, [FromBody] IngredientSuggestion ingredientSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
