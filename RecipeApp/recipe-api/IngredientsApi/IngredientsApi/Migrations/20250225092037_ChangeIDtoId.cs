using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngredientsApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIDtoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_ingredientID",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "recipeID",
                table: "Recipes",
                newName: "recipeId");

            migrationBuilder.RenameColumn(
                name: "ingredientID",
                table: "RecipeIngredients",
                newName: "ingredientId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "RecipeIngredients",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_ingredientID",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_ingredientId");

            migrationBuilder.RenameColumn(
                name: "ingredientID",
                table: "Ingredients",
                newName: "ingredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_ingredientId",
                table: "RecipeIngredients",
                column: "ingredientId",
                principalTable: "Ingredients",
                principalColumn: "ingredientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_ingredientId",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "recipeId",
                table: "Recipes",
                newName: "recipeID");

            migrationBuilder.RenameColumn(
                name: "ingredientId",
                table: "RecipeIngredients",
                newName: "ingredientID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RecipeIngredients",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredients_ingredientId",
                table: "RecipeIngredients",
                newName: "IX_RecipeIngredients_ingredientID");

            migrationBuilder.RenameColumn(
                name: "ingredientId",
                table: "Ingredients",
                newName: "ingredientID");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_ingredientID",
                table: "RecipeIngredients",
                column: "ingredientID",
                principalTable: "Ingredients",
                principalColumn: "ingredientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
