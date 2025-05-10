using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSite.Domains.Migrations
{
    /// <inheritdoc />
    public partial class PlaneRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "TravelClasses");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Planes",
                newName: "SecondClassCapacity");

            migrationBuilder.AddColumn<int>(
                name: "SeatClass",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstClassCapacity",
                table: "Planes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatClass",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FirstClassCapacity",
                table: "Planes");

            migrationBuilder.RenameColumn(
                name: "SecondClassCapacity",
                table: "Planes",
                newName: "Capacity");

            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TravelClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TravelCl__3214EC079AB0C82B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TravelClassId = table.Column<int>(type: "int", nullable: true),
                    SeatNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seats__3214EC07303D93A1", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Seats__TravelCla__31B762FC",
                        column: x => x.TravelClassId,
                        principalTable: "TravelClasses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_TravelClassId",
                table: "Seats",
                column: "TravelClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");
        }
    }
}
