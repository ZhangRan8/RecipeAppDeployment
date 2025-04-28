using Microsoft.AspNetCore.Mvc;
using RecipesApi.Models;
using RecipesApi.Services;

namespace RecipesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(IRecipeService recipeService, ILogger<RecipesController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipes(CancellationToken ct)
        {
            var recipes = await _recipeService.GetRecipesAsync(ct);
            return Ok(recipes);

        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipeById(int recipeId, CancellationToken ct)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(recipeId, ct);
            return Ok(recipe);

        }

        [HttpGet("limited")]
        public async Task<IActionResult> GetLimitedRecipes(CancellationToken ct, int limit = 3)
        {
            var recipes = await _recipeService.GetRecipesAsync(ct, limit);
            return Ok(recipes);
        }

        [HttpPost("/recipe/add")]
        public async Task<IActionResult> AddRecipe(RecipeDTO recipeDTO, CancellationToken ct)
        {
            var recipe = await _recipeService.AddRecipeAsync(recipeDTO, ct);
            return Ok(recipe);

        }

        [HttpPut("Edit/{recipeId}")]
        public async Task<IActionResult> EditRecipe(int recipeId, RecipeDTO recipeDTO, CancellationToken ct)
        {
            var recipe = await _recipeService.EditRecipeAsync(recipeDTO, ct);
            return Ok(recipe);
        }

        [HttpDelete("delete/{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(int recipeId, CancellationToken ct)
        {
            await _recipeService.DeleteRecipeAsync(recipeId, ct);
            _logger.LogInformation("Recipe with ID {RecipeId} deleted.", recipeId);
            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRecipe(string search, CancellationToken ct)
        {
            var recipe = await _recipeService.SearchRecipeByNameAsync(search, ct);
            return Ok(recipe);
        }

    }
}