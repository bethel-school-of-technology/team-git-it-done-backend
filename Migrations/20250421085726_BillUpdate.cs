using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FareShare_api.Migrations
{
    /// <inheritdoc />
    public partial class BillUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Settled",
                table: "BillLink",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SharedPrice",
                table: "Bill",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settled",
                table: "BillLink");

            migrationBuilder.DropColumn(
                name: "SharedPrice",
                table: "Bill");
        }
    }
}
