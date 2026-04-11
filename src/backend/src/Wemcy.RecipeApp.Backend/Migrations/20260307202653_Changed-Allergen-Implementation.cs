using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAllergenImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllergenRecipe");

            migrationBuilder.DropTable(
                name: "Allergens");
            // by the way, this drop everything related to the allergen, we dont care, fukc it, its still in dev

            migrationBuilder.AddColumn<int>(
                name: "Allergens",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergens",
                table: "Recipes");

            migrationBuilder.CreateTable(
                name: "Allergens",
                columns: table => new
                {
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergens", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "AllergenRecipe",
                columns: table => new
                {
                    AllergensType = table.Column<int>(type: "integer", nullable: false),
                    RecipesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergenRecipe", x => new { x.AllergensType, x.RecipesId });
                    table.ForeignKey(
                        name: "FK_AllergenRecipe_Allergens_AllergensType",
                        column: x => x.AllergensType,
                        principalTable: "Allergens",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllergenRecipe_Recipes_RecipesId",
                        column: x => x.RecipesId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Allergens",
                column: "Type",
                values: new object[]
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllergenRecipe_RecipesId",
                table: "AllergenRecipe",
                column: "RecipesId");
        }
    }
}
