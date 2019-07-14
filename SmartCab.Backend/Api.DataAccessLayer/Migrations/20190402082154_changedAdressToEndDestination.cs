using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class changedAdressToEndDestination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SlutDestination_StreetNumber",
                table: "Rides",
                newName: "EndDestination_StreetNumber");

            migrationBuilder.RenameColumn(
                name: "SlutDestination_StreetName",
                table: "Rides",
                newName: "EndDestination_StreetName");

            migrationBuilder.RenameColumn(
                name: "SlutDestination_PostalCode",
                table: "Rides",
                newName: "EndDestination_PostalCode");

            migrationBuilder.RenameColumn(
                name: "SlutDestination_CityName",
                table: "Rides",
                newName: "EndDestination_CityName");

            migrationBuilder.RenameColumn(
                name: "SlutDestinationId",
                table: "Rides",
                newName: "EndDestinationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDestination_StreetNumber",
                table: "Rides",
                newName: "SlutDestination_StreetNumber");

            migrationBuilder.RenameColumn(
                name: "EndDestination_StreetName",
                table: "Rides",
                newName: "SlutDestination_StreetName");

            migrationBuilder.RenameColumn(
                name: "EndDestination_PostalCode",
                table: "Rides",
                newName: "SlutDestination_PostalCode");

            migrationBuilder.RenameColumn(
                name: "EndDestination_CityName",
                table: "Rides",
                newName: "SlutDestination_CityName");

            migrationBuilder.RenameColumn(
                name: "EndDestinationId",
                table: "Rides",
                newName: "SlutDestinationId");
        }
    }
}
