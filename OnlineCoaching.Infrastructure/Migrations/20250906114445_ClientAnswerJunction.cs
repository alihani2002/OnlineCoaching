using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientAnswerJunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAnswerOption_ClientAnswers_ClientAnswerId",
                table: "ClientAnswerOption");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAnswerOption_Options_OptionId",
                table: "ClientAnswerOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientAnswerOption",
                table: "ClientAnswerOption");

            migrationBuilder.RenameTable(
                name: "ClientAnswerOption",
                newName: "ClientAnswerOptions");

            migrationBuilder.RenameIndex(
                name: "IX_ClientAnswerOption_OptionId",
                table: "ClientAnswerOptions",
                newName: "IX_ClientAnswerOptions_OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientAnswerOption_ClientAnswerId",
                table: "ClientAnswerOptions",
                newName: "IX_ClientAnswerOptions_ClientAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientAnswerOptions",
                table: "ClientAnswerOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAnswerOptions_ClientAnswers_ClientAnswerId",
                table: "ClientAnswerOptions",
                column: "ClientAnswerId",
                principalTable: "ClientAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAnswerOptions_Options_OptionId",
                table: "ClientAnswerOptions",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAnswerOptions_ClientAnswers_ClientAnswerId",
                table: "ClientAnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAnswerOptions_Options_OptionId",
                table: "ClientAnswerOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientAnswerOptions",
                table: "ClientAnswerOptions");

            migrationBuilder.RenameTable(
                name: "ClientAnswerOptions",
                newName: "ClientAnswerOption");

            migrationBuilder.RenameIndex(
                name: "IX_ClientAnswerOptions_OptionId",
                table: "ClientAnswerOption",
                newName: "IX_ClientAnswerOption_OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientAnswerOptions_ClientAnswerId",
                table: "ClientAnswerOption",
                newName: "IX_ClientAnswerOption_ClientAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientAnswerOption",
                table: "ClientAnswerOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAnswerOption_ClientAnswers_ClientAnswerId",
                table: "ClientAnswerOption",
                column: "ClientAnswerId",
                principalTable: "ClientAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAnswerOption_Options_OptionId",
                table: "ClientAnswerOption",
                column: "OptionId",
                principalTable: "Options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
