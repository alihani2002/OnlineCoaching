using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMealENtity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_Meal_MealId",
                table: "AssignFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_Meal_Clients_ClientId",
                table: "Meal");

            migrationBuilder.DropForeignKey(
                name: "FK_Meal_CoachingPackageRequests_CoachingPackageRequestId",
                table: "Meal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meal",
                table: "Meal");

            migrationBuilder.RenameTable(
                name: "Meal",
                newName: "Meals");

            migrationBuilder.RenameIndex(
                name: "IX_Meal_CoachingPackageRequestId",
                table: "Meals",
                newName: "IX_Meals_CoachingPackageRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Meal_ClientId",
                table: "Meals",
                newName: "IX_Meals_ClientId");

            migrationBuilder.AddColumn<int>(
                name: "MealNumber",
                table: "AssignFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meals",
                table: "Meals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_Meals_MealId",
                table: "AssignFoods",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Clients_ClientId",
                table: "Meals",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_CoachingPackageRequests_CoachingPackageRequestId",
                table: "Meals",
                column: "CoachingPackageRequestId",
                principalTable: "CoachingPackageRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_Meals_MealId",
                table: "AssignFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Clients_ClientId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_CoachingPackageRequests_CoachingPackageRequestId",
                table: "Meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meals",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "MealNumber",
                table: "AssignFoods");

            migrationBuilder.RenameTable(
                name: "Meals",
                newName: "Meal");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_CoachingPackageRequestId",
                table: "Meal",
                newName: "IX_Meal_CoachingPackageRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_ClientId",
                table: "Meal",
                newName: "IX_Meal_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meal",
                table: "Meal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_Meal_MealId",
                table: "AssignFoods",
                column: "MealId",
                principalTable: "Meal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Meal_Clients_ClientId",
                table: "Meal",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meal_CoachingPackageRequests_CoachingPackageRequestId",
                table: "Meal",
                column: "CoachingPackageRequestId",
                principalTable: "CoachingPackageRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
