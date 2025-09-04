using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMealENtity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealId",
                table: "AssignFoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfServings",
                table: "AssignFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealNumber = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CoachingPackageRequestId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meal_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meal_CoachingPackageRequests_CoachingPackageRequestId",
                        column: x => x.CoachingPackageRequestId,
                        principalTable: "CoachingPackageRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignFoods_MealId",
                table: "AssignFoods",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_ClientId",
                table: "Meal",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_CoachingPackageRequestId",
                table: "Meal",
                column: "CoachingPackageRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignFoods_Meal_MealId",
                table: "AssignFoods",
                column: "MealId",
                principalTable: "Meal",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignFoods_Meal_MealId",
                table: "AssignFoods");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropIndex(
                name: "IX_AssignFoods_MealId",
                table: "AssignFoods");

            migrationBuilder.DropColumn(
                name: "MealId",
                table: "AssignFoods");

            migrationBuilder.DropColumn(
                name: "NumberOfServings",
                table: "AssignFoods");
        }
    }
}
