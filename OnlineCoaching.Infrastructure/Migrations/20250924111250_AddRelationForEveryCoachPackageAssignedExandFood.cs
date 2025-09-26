using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationForEveryCoachPackageAssignedExandFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoachingPackageId",
                table: "AssignFoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoachingPackageId",
                table: "AssignExercises",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignFoods_CoachingPackageId",
                table: "AssignFoods",
                column: "CoachingPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignExercises_CoachingPackageId",
                table: "AssignExercises",
                column: "CoachingPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignExercises_CoachingPackages_CoachingPackageId",
                table: "AssignExercises",
                column: "CoachingPackageId",
                principalTable: "CoachingPackages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_CoachingPackages_CoachingPackageId",
                table: "AssignFoods",
                column: "CoachingPackageId",
                principalTable: "CoachingPackages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignExercises_CoachingPackages_CoachingPackageId",
                table: "AssignExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_CoachingPackages_CoachingPackageId",
                table: "AssignFoods");

            migrationBuilder.DropIndex(
                name: "IX_AssignFoods_CoachingPackageId",
                table: "AssignFoods");

            migrationBuilder.DropIndex(
                name: "IX_AssignExercises_CoachingPackageId",
                table: "AssignExercises");

            migrationBuilder.DropColumn(
                name: "CoachingPackageId",
                table: "AssignFoods");

            migrationBuilder.DropColumn(
                name: "CoachingPackageId",
                table: "AssignExercises");
        }
    }
}
