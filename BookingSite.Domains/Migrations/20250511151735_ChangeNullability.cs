using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSite.Domains.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Airports__CityId__2A164134",
                table: "Airports");

            migrationBuilder.DropForeignKey(
                name: "FK__Flights__PlaneId__3C34F170",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK__Flights__RouteId__3C34F16F",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK__Routes__FromAirp__3493CFA7",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK__Routes__ToAirpor__3587F3E0",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK__RouteSegm__Airpo__395884C4",
                table: "RouteSegments");

            migrationBuilder.DropForeignKey(
                name: "FK__RouteSegm__Route__3864608B",
                table: "RouteSegments");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceNumber",
                table: "RouteSegments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "RouteSegments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AirportId",
                table: "RouteSegments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ToAirportId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Routes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FromAirportId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlaneId",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Flights",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airports",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Airports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Airports__CityId__2A164134",
                table: "Airports",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Flights__PlaneId__3C34F170",
                table: "Flights",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Flights__RouteId__3C34F16F",
                table: "Flights",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Routes__FromAirp__3493CFA7",
                table: "Routes",
                column: "FromAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Routes__ToAirpor__3587F3E0",
                table: "Routes",
                column: "ToAirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK__RouteSegm__Airpo__395884C4",
                table: "RouteSegments",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__RouteSegm__Route__3864608B",
                table: "RouteSegments",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Airports__CityId__2A164134",
                table: "Airports");

            migrationBuilder.DropForeignKey(
                name: "FK__Flights__PlaneId__3C34F170",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK__Flights__RouteId__3C34F16F",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK__Routes__FromAirp__3493CFA7",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK__Routes__ToAirpor__3587F3E0",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK__RouteSegm__Airpo__395884C4",
                table: "RouteSegments");

            migrationBuilder.DropForeignKey(
                name: "FK__RouteSegm__Route__3864608B",
                table: "RouteSegments");

            migrationBuilder.AlterColumn<int>(
                name: "SequenceNumber",
                table: "RouteSegments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "RouteSegments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AirportId",
                table: "RouteSegments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ToAirportId",
                table: "Routes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Routes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "FromAirportId",
                table: "Routes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PlaneId",
                table: "Flights",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Flights",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Airports",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Airports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK__Airports__CityId__2A164134",
                table: "Airports",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Flights__PlaneId__3C34F170",
                table: "Flights",
                column: "PlaneId",
                principalTable: "Planes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Flights__RouteId__3C34F16F",
                table: "Flights",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Routes__FromAirp__3493CFA7",
                table: "Routes",
                column: "FromAirportId",
                principalTable: "Airports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Routes__ToAirpor__3587F3E0",
                table: "Routes",
                column: "ToAirportId",
                principalTable: "Airports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__RouteSegm__Airpo__395884C4",
                table: "RouteSegments",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__RouteSegm__Route__3864608B",
                table: "RouteSegments",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id");
        }
    }
}
