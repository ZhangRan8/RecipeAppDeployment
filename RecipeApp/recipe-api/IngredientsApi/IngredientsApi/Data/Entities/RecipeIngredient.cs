using IngredientsApi.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace RecipesApi.Models;
public class RecipeIngredient
{
    public int Id { get; set; }

    [ForeignKey("recipeId")]
    public int recipeId { get; set; }

    [ForeignKey("ingredientId")]
    public int ingredientId { get; set; }

    public double quantity { get; set; }
    public string unit { get; set; }

    public Recipe Recipe { get; set; }
    public Ingredient Ingredient { get; set; }
}
