using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Steps",
                table: "Recipes",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>());
                
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Steps",
                table: "Recipes");
        }
    }
}
