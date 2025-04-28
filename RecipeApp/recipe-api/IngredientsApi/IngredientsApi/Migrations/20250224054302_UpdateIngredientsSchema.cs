using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IngredientsApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIngredientsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "kilojoulesPerUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "unitOfMeasurement",
                table: "Ingredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "kilojoulesPerUnit",
                table: "Ingredients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "unitOfMeasurement",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
