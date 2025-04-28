using IngredientsApi.Data;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Models;

namespace RecipesApi.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _context;

        public RecipeService(AppDbContext context)
        {
            _context = context;
        }


        private RecipeDTO ConvertRecipeToDTO(Recipe recipe)
        {
            return new RecipeDTO
            {
                recipeId = recipe.recipeId,
                recipeName = recipe.recipeName,
                recipeImageUrl = recipe.recipeImageUrl,
                instructions = recipe.instructions,
                ingredients = recipe.RecipeIngredients.Select(ri => new RecipeIngredientDTO
                {
                    ingredientId = ri.ingredientId,
                    ingredientName = ri.Ingredient.ingredientName,
                    quantity = ri.quantity,
                    unit = ri.unit,
                    isActive = ri.Ingredient.isActive
                }).ToList()
            };
        }

        private IQueryable<RecipeDTO> GetRecipesQuery()
        {
            return _context.Recipes
                .Select(recipes => new RecipeDTO
                {
                    recipeId = recipes.recipeId,
                    recipeName = recipes.recipeName,
                    recipeImageUrl = recipes.recipeImageUrl,
                    instructions = recipes.instructions,
                    ingredients = recipes.RecipeIngredients.Select(ri => new RecipeIngredientDTO
                    {
                        ingredientId = ri.Ingredient.ingredientId,
                        ingredientName = ri.Ingredient.ingredientName,
                        quantity = ri.quantity,
                        unit = ri.unit,
                        isActive = ri.Ingredient.isActive
                    }).ToList()
                });
        }

        public void ValidateRecipe(RecipeDTO newRecipe)
        {
            if (newRecipe == null)
            {
                throw new ArgumentNullException(nameof(newRecipe), "New recipe cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(newRecipe.recipeName))
            {
                throw new ArgumentException("Recipe name cannot be null or empty.", nameof(newRecipe.recipeName));
            }

            if (newRecipe.recipeName.Length > 100)
            {
                throw new ArgumentException("Recipe name cannot exceed 100 characters.");
            }

            if (string.IsNullOrWhiteSpace(newRecipe.instructions))
            {
                throw new ArgumentException("Instructions cannot be null or empty.", nameof(newRecipe.instructions));
            }

            if (newRecipe.ingredients == null || newRecipe.ingredients.Count == 0)
            {
                throw new ArgumentException("A recipe must have at least one ingredient.");
            }

            foreach (var recipeIngredient in newRecipe.ingredients)
            {
                if (recipeIngredient.ingredientId < 0)
                {
                    throw new ArgumentException("Ingredient Id must be greater than 0.", nameof(recipeIngredient.ingredientId));
                }

                if (recipeIngredient.quantity <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than 0.", nameof(recipeIngredient.quantity));
                }

                if (string.IsNullOrWhiteSpace(recipeIngredient.unit))
                {
                    throw new ArgumentException("Unit cannot be null or empty.", nameof(recipeIngredient.unit));
                }
            }
        }

        public async Task CheckIngreidientsAreActiveAsync(IEnumerable<int> ingredientIds, CancellationToken ct)
        {
            var invalidIngredients = await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.ingredientId) && !i.isActive)
                .Select(i => i.ingredientName)
                .ToListAsync(ct);

            if (invalidIngredients.Any())
            {
                throw new ArgumentException($"The following ingredients are inactive and cannot be used: {string.Join(", ", invalidIngredients)}.");
            }
        }

        public async Task<RecipeDTO> GetRecipeByIdAsync(int recipeId, CancellationToken ct)
        {
            var recipe = await _context.Recipes
                .Where(r => r.recipeId == recipeId)
                .Select(r => new RecipeDTO
                {
                    recipeId = r.recipeId,
                    recipeName = r.recipeName,
                    recipeImageUrl = r.recipeImageUrl,
                    instructions = r.instructions,
                    ingredients = r.RecipeIngredients.Select(ri => new RecipeIngredientDTO
                    {
                        ingredientId = ri.Ingredient.ingredientId,
                        ingredientName = ri.Ingredient.ingredientName,
                        quantity = ri.quantity,
                        unit = ri.unit,
                        isActive = ri.Ingredient.isActive
                    }).ToList()
                }).FirstOrDefaultAsync(ct);

            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with Id {recipeId} not found.");
            }

            return recipe;
        }

        public async Task<List<RecipeDTO>> GetRecipesAsync(CancellationToken ct, int? limit = null)
        {
            var recipesQuery = GetRecipesQuery();
            if (limit.HasValue)
            {
                recipesQuery = recipesQuery.Take(limit.Value);
            }

            var recipes = await recipesQuery.ToListAsync(ct);
            return recipes;
        }


        public async Task<RecipeDTO> AddRecipeAsync(RecipeDTO recipeDTO, CancellationToken ct)
        {
            ValidateRecipe(recipeDTO);

            // Check if ingredients are active
            var ingredientIds = recipeDTO.ingredients.Select(ri => ri.ingredientId).ToList();
            await CheckIngreidientsAreActiveAsync(ingredientIds, ct);

            var recipe = new Recipe
            {
                recipeName = recipeDTO.recipeName,
                recipeImageUrl = recipeDTO.recipeImageUrl,
                instructions = recipeDTO.instructions
            };

            recipe.RecipeIngredients = recipeDTO.ingredients.Select(ri => new RecipeIngredient
            {
                ingredientId = ri.ingredientId,
                quantity = ri.quantity,
                unit = ri.unit
            }).ToList();

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync(ct);

            return ConvertRecipeToDTO(recipe);
        }

        public async Task<RecipeDTO> EditRecipeAsync(RecipeDTO recipeDTO, CancellationToken ct)
        {
            ValidateRecipe(recipeDTO);
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.recipeId == recipeDTO.recipeId, ct);

            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with Id {recipeDTO.recipeId} not found.");
            }

            recipe.recipeName = recipeDTO.recipeName;
            recipe.recipeImageUrl = recipeDTO.recipeImageUrl;
            recipe.instructions = recipeDTO.instructions;

            var existingIngredients = recipe.RecipeIngredients.ToList();
            var newIngredients = recipeDTO.ingredients;

            foreach (var existingIngredient in existingIngredients)
            {
                if (!newIngredients.Any(ni => ni.ingredientId == existingIngredient.ingredientId))
                {
                    _context.RecipeIngredients.Remove(existingIngredient);
                }
            }

            foreach (var newIngredient in newIngredients)
            {
                var existingIngredient = existingIngredients
                    .FirstOrDefault(ei => ei.ingredientId == newIngredient.ingredientId);

                if (existingIngredient == null)
                {
                    recipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        recipeId = recipe.recipeId,
                        ingredientId = newIngredient.ingredientId,
                        quantity = newIngredient.quantity,
                        unit = newIngredient.unit
                    });
                }
                else
                {
                    existingIngredient.quantity = newIngredient.quantity;
                    existingIngredient.unit = newIngredient.unit;
                }
            }

            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync(ct);

            return ConvertRecipeToDTO(recipe);
        }

        public async Task DeleteRecipeAsync(int recipeId, CancellationToken ct)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(r => r.recipeId == recipeId, ct);
            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with Id {recipeId} not found.");
            }
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<List<RecipeDTO>> SearchRecipeByNameAsync(string search, CancellationToken ct)
        {
            if (search == null)
            {
                throw new ArgumentNullException(nameof(search), "Search term cannot be null.");
            }
            var recipesQuery = GetRecipesQuery();
            recipesQuery = recipesQuery.Where(r => r.recipeName.Contains(search));

            var recipes = await recipesQuery.ToListAsync(ct);

            return recipes;
        }
    }
}