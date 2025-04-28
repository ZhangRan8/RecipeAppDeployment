using IngredientsApi.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecipesApi.Models;
using RecipesApi.Services;

namespace Recipe.UnitTests
{
    public class RecipeValidatorTests
    {

        private readonly IRecipeService _recipeService;

        public RecipeValidatorTests()
        {
            // Minimal mock setup for AppDbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            var mockDbContext = new Mock<AppDbContext>(options);

            _recipeService = new RecipeService(mockDbContext.Object);
        }

        [Fact]
        public void ValidateRecipe_NullRecipe_ThrowsArgumentNullException()
        {
            // Arrange
            RecipeDTO newRecipe = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.StartsWith("New recipe cannot be null.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_EmptyRecipeName_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "",
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = 1, quantity = 1, unit = "cup" }
            }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.StartsWith("Recipe name cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_RecipeNameTooLong_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = new string('A', 101), // 101 characters
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = 1, quantity = 1, unit = "cup" }
            }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.Equal("Recipe name cannot exceed 100 characters.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_NullInstructions_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "Test Recipe",
                instructions = null,
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = 1, quantity = 1, unit = "cup" }
            }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.StartsWith("Instructions cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_NoIngredients_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "Test Recipe",
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>()
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.Equal("A recipe must have at least one ingredient.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_NegativeIngredientId_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "Test Recipe",
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = -1, quantity = 1, unit = "cup" }
            }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.StartsWith("Ingredient Id must be greater than 0.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_ZeroQuantity_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "Test Recipe",
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = 1, quantity = 0, unit = "cup" }
            }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.StartsWith("Quantity must be greater than 0.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_EmptyUnit_ThrowsArgumentException()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "Test Recipe",
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = 1, quantity = 1, unit = "" }
            }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _recipeService.ValidateRecipe(newRecipe));
            Assert.StartsWith("Unit cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ValidateRecipe_ValidRecipe_DoesNotThrow()
        {
            // Arrange
            var newRecipe = new RecipeDTO
            {
                recipeName = "Test Recipe",
                instructions = "Test instructions",
                recipeImageUrl = "https://example.com/test.jpg",
                ingredients = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { ingredientId = 1, quantity = 1, unit = "cup" }
            }
            };

            // Act & Assert
            _recipeService.ValidateRecipe(newRecipe); // Should not throw any exception
        }


    }
}
