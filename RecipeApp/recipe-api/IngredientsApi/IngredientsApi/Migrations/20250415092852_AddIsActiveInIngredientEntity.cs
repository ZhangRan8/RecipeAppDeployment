using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngredientsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveInIngredientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Ingredients",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Ingredients");
        }
    }
}
