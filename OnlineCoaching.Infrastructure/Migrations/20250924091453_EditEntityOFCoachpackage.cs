using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCoaching.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditEntityOFCoachpackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachingPackageRequests_Clients_ClientId",
                table: "CoachingPackageRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsFreePlan",
                table: "CoachingPackages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "CoachingPackageRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingPackageRequests_Clients_ClientId",
                table: "CoachingPackageRequests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoachingPackageRequests_Clients_ClientId",
                table: "CoachingPackageRequests");

            migrationBuilder.DropColumn(
                name: "IsFreePlan",
                table: "CoachingPackages");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "CoachingPackageRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoachingPackageRequests_Clients_ClientId",
                table: "CoachingPackageRequests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
