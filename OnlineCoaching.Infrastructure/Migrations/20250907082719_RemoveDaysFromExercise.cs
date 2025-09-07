using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDaysFromExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Exercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
