namespace RecipesApi.Models
{
    public class RecipeIngredientDTO
    {
        public int ingredientId { get; set; }
        public string ingredientName { get; set; }
        public double quantity { get; set; }
        public string unit { get; set; }
        public bool isActive { get; set; }
    }
}