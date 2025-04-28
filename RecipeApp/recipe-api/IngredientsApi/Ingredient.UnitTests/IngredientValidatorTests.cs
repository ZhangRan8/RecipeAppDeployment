using IngredientsApi.Models;
using IngredientsApi.Services;

namespace Ingredient.UnitTests
{
    public class IngredientValidatorTests
    {
        private readonly IIngredientService _ingredientService = new IngredientService(null);

        [Fact]
        public void ValidateIngredient_NullIngredient_ThrowsArgumentNullException()
        {
            // Arrange
            IngredientDTO nullIngredient = null;
            // Act 
            var exception = Assert.Throws<ArgumentNullException>(() => _ingredientService.ValidateIngredient(nullIngredient));
            // Assert
            Assert.StartsWith("newIngredient", exception.ParamName);
        }

        [Fact]
        public void ValidateIngredient_EmptyIngredientName_ThrowsArgumentException()
        {
            // Arrange
            var emptyIngredient = new IngredientDTO
            {
                ingredientName = "",
                imageUrl = "http://example.com/tomato.jpg"
            };
            // Act 
            var exception = Assert.Throws<ArgumentException>(() => _ingredientService.ValidateIngredient(emptyIngredient));
            // Assert
            Assert.StartsWith("Ingredient name cannot be null or empty.", exception.Message);
        }

        [Fact]
        public void ValidateIngredient_IngredientNameTooLong_ThrowsArgumentException()
        {
            // Arrange
            var longIngredient = new IngredientDTO
            {
                ingredientName = new string('a', 101), // 101 characters long
                imageUrl = "http://example.com/tomato.jpg"
            };
            // Act 
            var exception = Assert.Throws<ArgumentException>(() => _ingredientService.ValidateIngredient(longIngredient));
            // Assert
            Assert.StartsWith("Ingredient name cannot exceed 100 characters.", exception.Message);
        }

        [Fact]
        public void ValidateIngredient_ValidIngredient_DoesNotThrow()
        {
            // Arrange
            var validIngredient = new IngredientDTO
            {
                ingredientName = "Tomato",
                imageUrl = "http://example.com/tomato.jpg"
            };
            // Act 
            _ingredientService.ValidateIngredient(validIngredient);

            // Assert
            // Should not throw any exception
        }
    }
}
