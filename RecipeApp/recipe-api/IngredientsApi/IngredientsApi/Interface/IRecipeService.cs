using RecipesApi.Models;

namespace RecipesApi.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeDTO>> GetRecipesAsync(CancellationToken ct, int? limit = null);
        Task<RecipeDTO> GetRecipeByIdAsync(int recipeId, CancellationToken ct);
        Task<RecipeDTO> AddRecipeAsync(RecipeDTO recipeDTO, CancellationToken ct);
        Task<RecipeDTO> EditRecipeAsync(RecipeDTO recipeDTO, CancellationToken ct);
        Task DeleteRecipeAsync(int recipeId, CancellationToken ct);
        Task<List<RecipeDTO>> SearchRecipeByNameAsync(string search, CancellationToken ct);
        void ValidateRecipe(RecipeDTO newRecipe);
    }
}