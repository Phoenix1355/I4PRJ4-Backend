using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class removedcustomerridestableandrenamedsharedopenridetosharedrides : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerRides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerRides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerId1 = table.Column<string>(nullable: true),
                    RideId = table.Column<int>(nullable: false),
                    TaxiCompanyId = table.Column<int>(nullable: false),
                    TaxiCompanyId1 = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerRides_AspNetUsers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerRides_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRides_AspNetUsers_TaxiCompanyId1",
                        column: x => x.TaxiCompanyId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_CustomerId1",
                table: "CustomerRides",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_RideId",
                table: "CustomerRides",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_TaxiCompanyId1",
                table: "CustomerRides",
                column: "TaxiCompanyId1");
        }
    }
}
