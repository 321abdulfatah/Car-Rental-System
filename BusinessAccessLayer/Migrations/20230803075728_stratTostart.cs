using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class stratTostart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StratDateRent",
                table: "Rentals",
                newName: "StartDateRent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDateRent",
                table: "Rentals",
                newName: "StratDateRent");
        }
    }
}
