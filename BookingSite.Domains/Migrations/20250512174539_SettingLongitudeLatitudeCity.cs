using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSite.Domains.Migrations
{
    /// <inheritdoc />
    public partial class SettingLongitudeLatitudeCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestId",
                table: "Cities");

            migrationBuilder.AddColumn<string>(
                name: "LatLong",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatLong",
                table: "Cities");

            migrationBuilder.AddColumn<int>(
                name: "DestId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
