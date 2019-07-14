using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class removedreftocustomerridestablefromcustomerandaddedreftoridetableinstead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Rides",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_CustomerId",
                table: "Rides",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_AspNetUsers_CustomerId",
                table: "Rides",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_AspNetUsers_CustomerId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_CustomerId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Rides");
        }
    }
}
