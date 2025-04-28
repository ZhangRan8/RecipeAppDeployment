namespace RecipesApi.Models
{
    public class RecipeDTO
    {
        public int recipeId { get; set; }
        public required string recipeName { get; set; }
        public required string recipeImageUrl { get; set; }
        public required string instructions { get; set; }
        public List<RecipeIngredientDTO> ingredients { get; set; }
    }
}