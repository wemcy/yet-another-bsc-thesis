using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Recipes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Extenstion = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ImageId",
                table: "Recipes",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Images_ImageId",
                table: "Recipes",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Images_ImageId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ImageId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Recipes");
        }
    }
}
