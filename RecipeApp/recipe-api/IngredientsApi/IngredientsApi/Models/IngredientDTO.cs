namespace IngredientsApi.Models
{
    public class IngredientDTO
    {
        public int ingredientId { get; set; }
        public required string ingredientName { get; set; }
        public required string imageUrl { get; set; }
        public bool isActive { get; set; }
    }
}