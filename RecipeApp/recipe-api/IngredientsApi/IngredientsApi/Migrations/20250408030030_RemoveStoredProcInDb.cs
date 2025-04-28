using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngredientsApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStoredProcInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS create_ingredient");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS update_ingredient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[create_ingredient]
                @ingredientName NVARCHAR(255),
                @imageUrl NVARCHAR(255)
            AS
            BEGIN
                INSERT INTO Ingredients (IngredientName, ImageUrl)
                VALUES (@ingredientName, @imageUrl);
            END;
            ");

            migrationBuilder.Sql(@"
            CREATE PROCEDURE [dbo].[update_ingredient]
                @ingredientId INT,          -- ID of the ingredient to update
                @ingredientName NVARCHAR(255),
                @imageUrl NVARCHAR(255)
            AS
            BEGIN
                UPDATE Ingredients
                SET IngredientName = @ingredientName, 
                    ImageUrl = @imageUrl
                WHERE IngredientId = @ingredientId;
            END;
            ");
        }
    }
}
