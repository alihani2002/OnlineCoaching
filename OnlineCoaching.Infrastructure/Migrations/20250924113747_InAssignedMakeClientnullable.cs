using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InAssignedMakeClientnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignExercises_Clients_ClientId",
                table: "AssignExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_Clients_ClientId",
                table: "AssignFoods");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "AssignFoods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "AssignExercises",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignExercises_Clients_ClientId",
                table: "AssignExercises",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_Clients_ClientId",
                table: "AssignFoods",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignExercises_Clients_ClientId",
                table: "AssignExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_Clients_ClientId",
                table: "AssignFoods");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "AssignFoods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "AssignExercises",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignExercises_Clients_ClientId",
                table: "AssignExercises",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_Clients_ClientId",
                table: "AssignFoods",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
