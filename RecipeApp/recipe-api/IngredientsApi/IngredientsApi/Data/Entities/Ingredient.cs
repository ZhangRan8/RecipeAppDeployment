namespace IngredientsApi.Models;
public class Ingredient
{
    public int ingredientId { get; set; }
    public required string ingredientName { get; set; }
    public string? imageUrl { get; set; }
    public Boolean isActive { get; set; } = true;
}