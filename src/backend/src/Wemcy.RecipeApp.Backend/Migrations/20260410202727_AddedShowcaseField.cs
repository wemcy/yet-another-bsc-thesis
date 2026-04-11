using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedShowcaseField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeShowcase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ShowcaseRecipeIds = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    FeaturedRecipeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeShowcase", x => x.Id);
                    table.CheckConstraint("CK_RecipeShowcase_Singleton", "\"Id\" = 1");
                    table.ForeignKey(
                        name: "FK_RecipeShowcase_Recipes_FeaturedRecipeId",
                        column: x => x.FeaturedRecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "RecipeShowcase",
                columns: new[] { "Id", "FeaturedRecipeId", "ShowcaseRecipeIds" },
                values: new object[] { 1, null, new Guid[0] });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeShowcase_FeaturedRecipeId",
                table: "RecipeShowcase",
                column: "FeaturedRecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeShowcase");
        }
    }
}
