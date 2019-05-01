using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class address_Lat_Lon_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EndDestination_Lat",
                table: "Rides",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EndDestination_Lng",
                table: "Rides",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartDestination_Lat",
                table: "Rides",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StartDestination_Lng",
                table: "Rides",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDestination_Lat",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "EndDestination_Lng",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestination_Lat",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "StartDestination_Lng",
                table: "Rides");
        }
    }
}
