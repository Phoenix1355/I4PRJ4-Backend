using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class AddedOwnershipAndRemovedEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Address_SlutDestinationId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Address_StartDestinationId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Rides_SlutDestinationId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_StartDestinationId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "SlutDestination_CityName",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlutDestination_PostalCode",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SlutDestination_StreetName",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlutDestination_StreetNumber",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StartDestination_CityName",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartDestination_PostalCode",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StartDestination_StreetName",
                table: "Rides",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartDestination_StreetNumber",
                table: "Rides",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlutDestination_CityName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SlutDestination_PostalCode",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SlutDestination_StreetName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SlutDestination_StreetNumber",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestination_CityName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestination_PostalCode",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestination_StreetName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestination_StreetNumber",
                table: "Rides");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Customers",
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

            migrationBuilder.CreateIndex(
                name: "IX_Rides_SlutDestinationId",
                table: "Rides",
                column: "SlutDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_StartDestinationId",
                table: "Rides",
                column: "StartDestinationId");

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
        }
    }
}
