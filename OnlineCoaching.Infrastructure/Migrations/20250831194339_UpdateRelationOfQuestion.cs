using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationOfQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAnswers_Options_OptionId",
                table: "ClientAnswers");

            migrationBuilder.DropIndex(
                name: "IX_ClientAnswers_OptionId",
                table: "ClientAnswers");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "ClientAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Options",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Options",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "ClientAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientAnswers_OptionId",
                table: "ClientAnswers",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAnswers_Options_OptionId",
                table: "ClientAnswers",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id");
        }
    }
}
