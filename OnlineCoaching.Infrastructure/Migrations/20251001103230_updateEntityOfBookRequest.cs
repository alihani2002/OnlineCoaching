using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateEntityOfBookRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityNumber",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "BookRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityNumber",
                table: "BookRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "BookRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
