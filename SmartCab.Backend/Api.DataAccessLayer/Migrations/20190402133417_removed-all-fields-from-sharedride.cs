using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class removedallfieldsfromsharedride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides",
                column: "MatchedRidesId",
                principalTable: "MatchedRides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides",
                column: "MatchedRidesId",
                principalTable: "MatchedRides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
