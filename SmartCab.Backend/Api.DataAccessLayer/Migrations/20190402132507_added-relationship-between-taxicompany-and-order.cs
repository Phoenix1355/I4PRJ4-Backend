using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class addedrelationshipbetweentaxicompanyandorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxiCompanyId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TaxiCompanyId",
                table: "Orders",
                column: "TaxiCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_TaxiCompanyId",
                table: "Orders",
                column: "TaxiCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_TaxiCompanyId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TaxiCompanyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TaxiCompanyId",
                table: "Orders");
        }
    }
}
