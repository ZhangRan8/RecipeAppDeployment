using IngredientsApi.Models;

namespace IngredientsApi.Services
{
    public interface IIngredientService
    {
        Task<List<IngredientDTO>> GetIngredientsAsync(CancellationToken ct);
        Task<IngredientDTO> GetIngredientByIdAsync(int ingredientId, CancellationToken ct);
        Task<IngredientDTO> GetIngredientByNameAsync(string ingredientName, CancellationToken ct);
        Task DeleteIngredientAsync(int ingredientId, CancellationToken ct);
        Task<IngredientDTO> AddIngredientAsync(IngredientDTO newIngredientDTO, CancellationToken ct);
        Task<List<IngredientDTO>> SearchIngredientsAsync(string search, CancellationToken ct);
        Task<IngredientDTO> EditIngredientAsync(int ingredientId, IngredientDTO editIngredient, CancellationToken ct);
        void ValidateIngredient(IngredientDTO newIngredient);
    }
}
