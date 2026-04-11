using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedSearchIndexForTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "TitleSearchVector",
                table: "Recipes",
                type: "tsvector",
                nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "Hungarian")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Title" });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_TitleSearchVector",
                table: "Recipes",
                column: "TitleSearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipes_TitleSearchVector",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "TitleSearchVector",
                table: "Recipes");
        }
    }
}
