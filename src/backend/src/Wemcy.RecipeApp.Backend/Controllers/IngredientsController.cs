using Wemcy.RecipeApp.Backend.Api.Controllers;
using Wemcy.RecipeApp.Backend.Model.Entities;
using Wemcy.RecipeApp.Backend.Security;
using Wemcy.RecipeApp.Backend.Services;

namespace Wemcy.RecipeApp.Backend.Controllers
{
    public class IngredientsController(IngredientSuggestionService ingredientSuggestionService, IMapper mapper) : IngredientsApiController
    {
        private readonly IngredientSuggestionService _ingredientSuggestionService = ingredientSuggestionService;
        private readonly IMapper _mapper = mapper;


        [Authorize(Roles = Roles.Admin)]
        public async override Task<IActionResult> CreateIngredient([FromBody] Api.Models.CreateIngredientSuggestionRequest ingredientSuggestion)
        {
            var ingredentSuggestion = _mapper.Map<IngredientSuggestion>(ingredientSuggestion);
            var newIngredient = await _ingredientSuggestionService.CreateIngredientAsync(ingredentSuggestion);
            return Ok(_mapper.Map<Api.Models.IngredientSuggestion>(newIngredient));
        }

        [Authorize(Roles = Roles.Admin)]
        public async override Task<IActionResult> DeleteIngredient([FromRoute(Name = "id"), Required] Guid id)
        {
            await _ingredientSuggestionService.DeleteIngredientAsync(id);
            return NoContent();
        }

        public async override Task<IActionResult> GetIngredientById([FromRoute(Name = "id"), Required] Guid id)
        {
            var ingredentSuggestion = await _ingredientSuggestionService.GetIngredientByIdAsync(id);
            return Ok(_mapper.Map<Api.Models.IngredientSuggestion>(ingredentSuggestion));
        }

        public async override Task<IActionResult> SearchIngredients([FromQuery(Name = "name"), Required] string name)
        {
            var result = _ingredientSuggestionService.SearchIngredientsAsAsync<Api.Models.IngredientSuggestion>(name);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        public async override Task<IActionResult> UpdateIngredient([FromRoute(Name = "id"), Required] Guid id, [FromBody] Api.Models.CreateIngredientSuggestionRequest ingredientSuggestion)
        {
            var ingredentSuggestion = await _ingredientSuggestionService.UpdateIngredientByIdAsync(id,ingredientSuggestion);
            return Ok(_mapper.Map<Api.Models.IngredientSuggestion>(ingredentSuggestion));

        }
    }
}
