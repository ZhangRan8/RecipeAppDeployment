using IngredientsApi.Models;
using IngredientsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IngredientsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientsController> _logger;

        public IngredientsController(IIngredientService ingredientService, ILogger<IngredientsController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        // GET /Ingredients
        [HttpGet]
        public async Task<IActionResult> GetIngredients(CancellationToken ct)
        {

            var ingredients = await _ingredientService.GetIngredientsAsync(ct);
            return Ok(ingredients);

        }

        // GET /Ingredients/{ingredientId}
        [HttpGet("{ingredientId}")]
        public async Task<IActionResult> GetIngredientById(int ingredientId, CancellationToken ct)
        {

            var ingredient = await _ingredientService.GetIngredientByIdAsync(ingredientId, ct);
            return Ok(ingredient);

        }

        // DELETE /Ingredients/{ingredientId}
        [HttpDelete("{ingredientId}")]
        public async Task<IActionResult> DeleteIngredient(int ingredientId, CancellationToken ct)
        {

            await _ingredientService.DeleteIngredientAsync(ingredientId, ct);
            return NoContent();

        }

        // POST /Ingredients
        [HttpPost("add")]
        public async Task<IActionResult> AddIngredient(IngredientDTO newIngredientDTO, CancellationToken ct)
        {

            var ingredient = await _ingredientService.AddIngredientAsync(newIngredientDTO, ct);
            return CreatedAtAction(nameof(GetIngredientById), new { ingredientId = ingredient.ingredientId }, ingredient);

        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchIngredients(string search, CancellationToken ct)
        {

            var ingredients = await _ingredientService.SearchIngredientsAsync(search, ct);
            return Ok(ingredients);

        }

        [HttpPut("Edit/{ingredientId}")]
        public async Task<IActionResult> EditIngredient(int ingredientId, IngredientDTO editIngredientDTO, CancellationToken ct)
        {

            var ingredient = await _ingredientService.EditIngredientAsync(ingredientId, editIngredientDTO, ct);
            return Ok(ingredient);

        }
    }
}

