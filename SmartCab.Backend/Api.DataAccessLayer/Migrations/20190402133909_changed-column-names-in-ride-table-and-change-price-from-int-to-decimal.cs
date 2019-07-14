using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class changedcolumnnamesinridetableandchangepricefrominttodecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
