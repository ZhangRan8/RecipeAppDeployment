namespace RecipesApi.Models;
public class Recipe
{
    public int recipeId { get; set; }
    public required string recipeName { get; set; }
    public string? recipeImageUrl { get; set; }
    public required string instructions { get; set; }
    public List<RecipeIngredient> RecipeIngredients { get; set; }
}