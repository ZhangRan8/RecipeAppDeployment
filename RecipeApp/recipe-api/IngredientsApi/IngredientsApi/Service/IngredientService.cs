using IngredientsApi.Data;
using IngredientsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IngredientsApi.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly AppDbContext _context;

        public IngredientService(AppDbContext context)
        {
            _context = context;
        }

        private List<IngredientDTO> ConvertIngredientsToDTOs(List<Ingredient> ingredients)
        {
            return ingredients.Select(i => new IngredientDTO
            {
                ingredientId = i.ingredientId,
                ingredientName = i.ingredientName,
                imageUrl = i.imageUrl,
                isActive = i.isActive
            }).ToList();
        }

        private IngredientDTO ConvertIngredientToDTO(Ingredient ingredient)
        {
            return new IngredientDTO
            {
                ingredientId = ingredient.ingredientId,
                ingredientName = ingredient.ingredientName,
                imageUrl = ingredient.imageUrl,
                isActive = ingredient.isActive
            };
        }

        public void ValidateIngredient(IngredientDTO newIngredient)
        {
            if (newIngredient == null)
            {
                throw new ArgumentNullException(nameof(newIngredient), "New ingredient cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(newIngredient.ingredientName))
            {
                throw new ArgumentException("Ingredient name cannot be null or empty.", nameof(newIngredient.ingredientName));
            }

            if (newIngredient.ingredientName.Length > 100)
            {
                throw new ArgumentException("Ingredient name cannot exceed 100 characters.");
            }


        }
        public async Task<IngredientDTO> GetIngredientByNameAsync(string ingredientName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(ingredientName))
            {
                throw new ArgumentException("Ingredient name cannot be null or empty.", nameof(ingredientName));
            }

            var ingredient = await _context.Ingredients.Where(i => i.isActive).FirstOrDefaultAsync(i => i.ingredientName == ingredientName, ct);
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with name {ingredientName} not found.");
            }

            return ConvertIngredientToDTO(ingredient);
        }

        public async Task<List<IngredientDTO>> GetIngredientsAsync(CancellationToken ct)
        {
            var ingredients = await _context.Ingredients.Where(i => i.isActive).ToListAsync(ct);
            return ConvertIngredientsToDTOs(ingredients);
        }

        public async Task<IngredientDTO> GetIngredientByIdAsync(int ingredientId, CancellationToken ct)
        {
            if (ingredientId <= 0)
            {
                throw new ArgumentException("Ingredient Id isn't valid.", nameof(ingredientId));
            }

            var ingredient = await _context.Ingredients.Where(i => i.isActive).FirstOrDefaultAsync(i => i.ingredientId == ingredientId, ct);

            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with Id {ingredientId} not found.");
            }

            return ConvertIngredientToDTO(ingredient);
        }

        public async Task DeleteIngredientAsync(int ingredientId, CancellationToken ct)
        {
            if (ingredientId <= 0)
            {
                throw new ArgumentException("Ingredient Id doesn't exist.", nameof(ingredientId));
            }

            var ingredient = await _context.Ingredients.Where(i => i.isActive).FirstOrDefaultAsync(i => i.ingredientId == ingredientId, ct);

            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with Id {ingredientId} not found.");
            }

            ingredient.isActive = false; // Soft delete
            await _context.SaveChangesAsync(ct);
        }


        public async Task<IngredientDTO> AddIngredientAsync(IngredientDTO newIngredient, CancellationToken ct)
        {
            ValidateIngredient(newIngredient);

            // Check if the ingredient already exists
            var existingIngredient = await _context.Ingredients
                .FirstOrDefaultAsync(i => i.ingredientName == newIngredient.ingredientName, ct);
            if (existingIngredient != null && !existingIngredient.isActive)
            {
                existingIngredient.isActive = true; // Reactivate the ingredient
                await _context.SaveChangesAsync(ct);
                return ConvertIngredientToDTO(existingIngredient);
            }

            var ingredient = new Ingredient
            {
                ingredientName = newIngredient.ingredientName.Trim(),
                imageUrl = newIngredient.imageUrl,
                isActive = true
            };

            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync(ct);

            return ConvertIngredientToDTO(ingredient);
        }


        public async Task<List<IngredientDTO>> SearchIngredientsAsync(string search, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new ArgumentException("Search term cannot be null or empty.", nameof(search));
            }

            var trimmedSearch = search.Trim();
            var ingredients = await _context.Ingredients.Where(
                i => i.ingredientName.Contains(trimmedSearch) && i.isActive).ToListAsync(ct);

            return ConvertIngredientsToDTOs(ingredients);
        }

        public async Task<IngredientDTO> EditIngredientAsync(int ingredientId, IngredientDTO editIngredient, CancellationToken ct)
        {

            ValidateIngredient(editIngredient);

            var ingredient = await _context.Ingredients.FindAsync(ingredientId);

            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with Id {ingredientId} not found.");
            }

            ingredient.ingredientName = editIngredient.ingredientName.Trim();
            ingredient.imageUrl = editIngredient.imageUrl;

            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync(ct);

            return ConvertIngredientToDTO(ingredient);
        }
    }
}