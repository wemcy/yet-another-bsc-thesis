using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Wemcy.RecipeApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRecipeUserRequirmentForRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Allergens", "AverageRating", "CreatedAt", "Description", "ImageId", "Steps", "Title", "UpdatedAt", "UserId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), 138, 0.0, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "A classic Italian pasta dish made with eggs, cheese, pancetta, and pepper.", null, new[] { "Cook spaghetti according to package instructions.", "In a separate pan, cook pancetta until crispy.", "In a bowl, whisk together eggs and grated cheese.", "Drain spaghetti and return to pot. Mix in pancetta and egg mixture quickly.", "Season with pepper and serve immediately." }, "Spaghetti Carbonara", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
