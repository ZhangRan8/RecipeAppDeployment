using IngredientsApi.Data;
using IngredientsApi.Models;
using IngredientsApi.Services;
using IntegrationTests.CommonFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ingredient.IntegrationTests
{
    public class AddIngredientTests : BaseIntegrationTest, IAsyncLifetime
    {
        private readonly IngredientService _ingredientService;
        private readonly AppDbContext _dbContext;

        public AddIngredientTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
            _ingredientService = _scope.ServiceProvider.GetRequiredService<IngredientService>();
            _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        public async Task InitializeAsync()
        {

        }

        public async Task DisposeAsync()
        {
            _dbContext.Ingredients.RemoveRange(_dbContext.Ingredients);
            await _dbContext.SaveChangesAsync();
        }



        [Fact]
        public async Task AddIngredientAsync_NewIngredient_AddsToDatabaseAndReturnsDTO()
        {
            // Arrange  
            var newIngredient = new IngredientDTO
            {
                ingredientName = "Sugar",
                imageUrl = "https://example.com/sugar.jpg"
            };
            var ct = CancellationToken.None;


            // Act  
            var result = await _ingredientService.AddIngredientAsync(newIngredient, ct);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal("Sugar", result.ingredientName);
            Assert.Equal("https://example.com/sugar.jpg", result.imageUrl);

            var dbIngredient = await _dbContext.Ingredients
                .FirstOrDefaultAsync(i => i.ingredientName == "Sugar", ct);
            Assert.NotNull(dbIngredient);
            Assert.True(dbIngredient.isActive);
            Assert.Equal("https://example.com/sugar.jpg", dbIngredient.imageUrl);
        }

        [Fact]
        public async Task Database_HasOneRecord()
        {
            // arrange
            var ct = CancellationToken.None;
            var ingredient = new IngredientDTO { ingredientName = "Salt", imageUrl = "https://example.com/salt.jpg" };
            await _ingredientService.AddIngredientAsync(ingredient, ct);

            // act
            var ingredientCount = await _dbContext.Ingredients.CountAsync(ct);

            // assert
            Assert.Equal(1, ingredientCount);
        }

        [Fact]
        public async Task Database_HasOneRecord_v2()
        {
            // arrange
            var ct = CancellationToken.None;
            var ingredient = new IngredientDTO { ingredientName = "bread", imageUrl = "https://example.com/bread.jpg" };
            await _ingredientService.AddIngredientAsync(ingredient, ct);

            // act
            var ingredientCount = await _dbContext.Ingredients.CountAsync(ct);

            // assert
            Assert.Equal(1, ingredientCount);
        }


    }
}
