using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransformationAddOneimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterImageUrl",
                table: "Transformations");

            migrationBuilder.RenameColumn(
                name: "BeforeImageUrl",
                table: "Transformations",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Transformations",
                newName: "BeforeImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "AfterImageUrl",
                table: "Transformations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
