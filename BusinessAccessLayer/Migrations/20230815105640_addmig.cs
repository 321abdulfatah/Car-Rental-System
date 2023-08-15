using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isAvailable",
                table: "Drivers",
                newName: "IsAvailable");

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Drivers",
                type: "uniqueidentifier",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Drivers_DriverId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_DriverId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Drivers",
                newName: "isAvailable");
        }
    }
}
