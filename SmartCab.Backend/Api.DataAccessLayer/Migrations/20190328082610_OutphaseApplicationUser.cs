using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.DataAccessLayer.Migrations
{
    public partial class OutphaseApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRideses_Customers_CustomerId",
                table: "CustomerRides");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRideses_TaxiCompanies_TaxiCompanyId",
                table: "CustomerRides");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "TaxiCompanies");

            migrationBuilder.DropIndex(
                name: "IX_CustomerRideses_CustomerId",
                table: "CustomerRides");

            migrationBuilder.DropIndex(
                name: "IX_CustomerRideses_TaxiCompanyId",
                table: "CustomerRides");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "CustomerRides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxiCompanyId1",
                table: "CustomerRides",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxiCompany_Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_CustomerId1",
                table: "CustomerRides",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_TaxiCompanyId1",
                table: "CustomerRides",
                column: "TaxiCompanyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRides_AspNetUsers_CustomerId1",
                table: "CustomerRides",
                column: "CustomerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRides_AspNetUsers_TaxiCompanyId1",
                table: "CustomerRides",
                column: "TaxiCompanyId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRides_AspNetUsers_CustomerId1",
                table: "CustomerRides");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerRides_AspNetUsers_TaxiCompanyId1",
                table: "CustomerRides");

            migrationBuilder.DropIndex(
                name: "IX_CustomerRides_CustomerId1",
                table: "CustomerRides");

            migrationBuilder.DropIndex(
                name: "IX_CustomerRides_TaxiCompanyId1",
                table: "CustomerRides");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "CustomerRides");

            migrationBuilder.DropColumn(
                name: "TaxiCompanyId1",
                table: "CustomerRides");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaxiCompany_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxiCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxiCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxiCompanies_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_CustomerId",
                table: "CustomerRides",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRides_TaxiCompanyId",
                table: "CustomerRides",
                column: "TaxiCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ApplicationUserId",
                table: "Customers",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxiCompanies_ApplicationUserId",
                table: "TaxiCompanies",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRides_Customers_CustomerId",
                table: "CustomerRides",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerRides_TaxiCompanies_TaxiCompanyId",
                table: "CustomerRides",
                column: "TaxiCompanyId",
                principalTable: "TaxiCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
