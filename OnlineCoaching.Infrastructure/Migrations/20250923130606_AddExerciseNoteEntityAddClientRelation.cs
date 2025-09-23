using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseNoteEntityAddClientRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseNotes_Clients_ClientId",
                table: "ExerciseNotes");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "ExerciseNotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseNotes_Clients_ClientId",
                table: "ExerciseNotes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseNotes_Clients_ClientId",
                table: "ExerciseNotes");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "ExerciseNotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseNotes_Clients_ClientId",
                table: "ExerciseNotes",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
