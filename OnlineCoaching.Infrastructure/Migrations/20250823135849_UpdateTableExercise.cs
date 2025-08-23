using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkUrls",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Rest",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "LinkUrls",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Rest",
                table: "Exercises");
        }
    }
}
