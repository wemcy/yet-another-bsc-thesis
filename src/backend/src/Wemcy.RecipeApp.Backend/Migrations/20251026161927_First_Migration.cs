using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class First_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenRecipe_Allergen_AllergensType",
                table: "AllergenRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Allergen",
                table: "Allergen");

            migrationBuilder.RenameTable(
                name: "Allergen",
                newName: "Allergens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Allergens",
                table: "Allergens",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenRecipe_Allergens_AllergensType",
                table: "AllergenRecipe",
                column: "AllergensType",
                principalTable: "Allergens",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllergenRecipe_Allergens_AllergensType",
                table: "AllergenRecipe");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Allergens",
                table: "Allergens");

            migrationBuilder.RenameTable(
                name: "Allergens",
                newName: "Allergen");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Allergen",
                table: "Allergen",
                column: "Type");

            migrationBuilder.AddForeignKey(
                name: "FK_AllergenRecipe_Allergen_AllergensType",
                table: "AllergenRecipe",
                column: "AllergensType",
                principalTable: "Allergen",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
