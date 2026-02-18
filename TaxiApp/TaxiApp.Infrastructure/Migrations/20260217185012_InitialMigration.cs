using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxiRides",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PickupDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DropoffDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassengerCount = table.Column<byte>(type: "tinyint", nullable: false),
                    TripDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StoreAndFwdFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PULocationId = table.Column<int>(type: "int", nullable: false),
                    DOLocationId = table.Column<int>(type: "int", nullable: false),
                    FareAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxiRides", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxiRides");
        }
    }
}
