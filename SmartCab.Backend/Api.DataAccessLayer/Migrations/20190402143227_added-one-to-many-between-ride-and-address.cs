using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class addedonetomanybetweenrideandaddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDestination_CityName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "EndDestination_PostalCode",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "EndDestination_StreetName",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "EndDestination_StreetNumber",
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

            migrationBuilder.AlterColumn<int>(
                name: "StartDestinationId",
                table: "Rides",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EndDestinationId",
                table: "Rides",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityName = table.Column<string>(nullable: false),
                    PostalCode = table.Column<int>(nullable: false),
                    StreetName = table.Column<string>(nullable: false),
                    StreetNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_EndDestinationId",
                table: "Rides",
                column: "EndDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_StartDestinationId",
                table: "Rides",
                column: "StartDestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Address_EndDestinationId",
                table: "Rides",
                column: "EndDestinationId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Address_EndDestinationId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Address_StartDestinationId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Rides_EndDestinationId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_StartDestinationId",
                table: "Rides");

            migrationBuilder.AlterColumn<int>(
                name: "StartDestinationId",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EndDestinationId",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndDestination_CityName",
                table: "Rides",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EndDestination_PostalCode",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EndDestination_StreetName",
                table: "Rides",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EndDestination_StreetNumber",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StartDestination_CityName",
                table: "Rides",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StartDestination_PostalCode",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StartDestination_StreetName",
                table: "Rides",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StartDestination_StreetNumber",
                table: "Rides",
                nullable: false,
                defaultValue: 0);
        }
    }
}
