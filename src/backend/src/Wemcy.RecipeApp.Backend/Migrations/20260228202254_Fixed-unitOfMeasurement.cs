using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class FixedunitOfMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitOfMesurement",
                table: "Ingredient",
                newName: "UnitOfMeasurement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitOfMeasurement",
                table: "Ingredient",
                newName: "UnitOfMesurement");
        }
    }
}
