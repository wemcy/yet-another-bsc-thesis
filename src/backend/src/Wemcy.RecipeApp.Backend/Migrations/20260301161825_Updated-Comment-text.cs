using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommenttext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Comment",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comment",
                newName: "Text");
        }
    }
}
