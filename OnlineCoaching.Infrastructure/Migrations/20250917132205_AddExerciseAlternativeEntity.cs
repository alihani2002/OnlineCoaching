using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseAlternativeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseAlternatives",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    AlternativeExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseAlternatives", x => new { x.ExerciseId, x.AlternativeExerciseId });
                    table.ForeignKey(
                        name: "FK_ExerciseAlternatives_Exercises_AlternativeExerciseId",
                        column: x => x.AlternativeExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseAlternatives_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseAlternatives_AlternativeExerciseId",
                table: "ExerciseAlternatives",
                column: "AlternativeExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseAlternatives");
        }
    }
}
