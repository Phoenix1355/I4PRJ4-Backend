using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class Initial_Design : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PassengerCount",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Rides",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Rides",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationDeadline",
                table: "Rides",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlutDestinationId",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartDestinationId",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchedRidesId",
                table: "Rides",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityName = table.Column<string>(nullable: true),
                    PostalCode = table.Column<int>(nullable: false),
                    StreetName = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    RideId = table.Column<int>(nullable: false),
                    TaxiCompanyId = table.Column<int>(nullable: false),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRideses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerRideses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRideses_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRideses_TaxiCompanies_TaxiCompanyId",
                        column: x => x.TaxiCompanyId,
                        principalTable: "TaxiCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchedRides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchedRides", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_SlutDestinationId",
                table: "Rides",
                column: "SlutDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_StartDestinationId",
                table: "Rides",
                column: "StartDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_MatchedRidesId",
                table: "Rides",
                column: "MatchedRidesId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRideses_CustomerId",
                table: "CustomerRides",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRideses_RideId",
                table: "CustomerRides",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRideses_TaxiCompanyId",
                table: "CustomerRides",
                column: "TaxiCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Address_SlutDestinationId",
                table: "Rides",
                column: "SlutDestinationId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Address_StartDestinationId",
                table: "Rides",
                column: "StartDestinationId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides",
                column: "MatchedRidesId",
                principalTable: "MatchedRides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Address_SlutDestinationId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Address_StartDestinationId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "CustomerRides");

            migrationBuilder.DropTable(
                name: "MatchedRides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_SlutDestinationId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_StartDestinationId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_MatchedRidesId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "PassengerCount",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "ConfirmationDeadline",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SlutDestinationId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestinationId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "MatchedRidesId",
                table: "Rides");
        }
    }
}
