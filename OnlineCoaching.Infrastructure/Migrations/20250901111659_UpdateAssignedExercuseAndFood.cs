using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignedExercuseAndFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoachingPackageRequestId",
                table: "AssignFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoachingPackageRequestId",
                table: "AssignExercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignFoods_CoachingPackageRequestId",
                table: "AssignFoods",
                column: "CoachingPackageRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignExercises_CoachingPackageRequestId",
                table: "AssignExercises",
                column: "CoachingPackageRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignExercises_CoachingPackageRequests_CoachingPackageRequestId",
                table: "AssignExercises",
                column: "CoachingPackageRequestId",
                principalTable: "CoachingPackageRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_CoachingPackageRequests_CoachingPackageRequestId",
                table: "AssignFoods",
                column: "CoachingPackageRequestId",
                principalTable: "CoachingPackageRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignExercises_CoachingPackageRequests_CoachingPackageRequestId",
                table: "AssignExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_CoachingPackageRequests_CoachingPackageRequestId",
                table: "AssignFoods");

            migrationBuilder.DropIndex(
                name: "IX_AssignFoods_CoachingPackageRequestId",
                table: "AssignFoods");

            migrationBuilder.DropIndex(
                name: "IX_AssignExercises_CoachingPackageRequestId",
                table: "AssignExercises");

            migrationBuilder.DropColumn(
                name: "CoachingPackageRequestId",
                table: "AssignFoods");

            migrationBuilder.DropColumn(
                name: "CoachingPackageRequestId",
                table: "AssignExercises");
        }
    }
}
