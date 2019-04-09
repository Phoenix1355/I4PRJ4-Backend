using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class reduntantCustomerIdRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_AspNetUsers_CustomerId1",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_CustomerId1",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Rides");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int));

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
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_AspNetUsers_CustomerId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_CustomerId",
                table: "Rides");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "Rides",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_CustomerId1",
                table: "Rides",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_AspNetUsers_CustomerId1",
                table: "Rides",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
