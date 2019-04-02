using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class removedmatchedridetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "MatchedRides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_MatchedRidesId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "MatchedRidesId",
                table: "Rides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchedRidesId",
                table: "Rides",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MatchedRides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchedRides", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_MatchedRidesId",
                table: "Rides",
                column: "MatchedRidesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_MatchedRides_MatchedRidesId",
                table: "Rides",
                column: "MatchedRidesId",
                principalTable: "MatchedRides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
