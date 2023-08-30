using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updatedriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Drivers_DriverId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_DriverId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "DriverId",
                table: "Drivers",
                newName: "ReplacmentDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ReplacmentDriverId",
                table: "Drivers",
                column: "ReplacmentDriverId",
                unique: true,
                filter: "[ReplacmentDriverId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Drivers_ReplacmentDriverId",
                table: "Drivers",
                column: "ReplacmentDriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Drivers_ReplacmentDriverId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ReplacmentDriverId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "ReplacmentDriverId",
                table: "Drivers",
                newName: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_DriverId",
                table: "Drivers",
                column: "DriverId",
                unique: true,
                filter: "[DriverId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Drivers_DriverId",
                table: "Drivers",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }
    }
}
