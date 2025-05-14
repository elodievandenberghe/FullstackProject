using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSite.Domains.Migrations
{
    /// <inheritdoc />
    public partial class addCreatedDateToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Bookings");
        }
    }
}
