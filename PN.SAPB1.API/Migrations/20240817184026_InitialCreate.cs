using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PN.SAPB1.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPartners",
                columns: table => new
                {
                    CardCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartners", x => x.CardCode);
                });

            migrationBuilder.CreateTable(
                name: "Bpaddresses",
                columns: table => new
                {
                    AddressName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BPCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Block = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    County = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FederalTaxID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingFloorRoom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressName3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bpaddresses", x => new { x.BPCode, x.AddressName });
                    table.ForeignKey(
                        name: "FK_Bpaddresses_BusinessPartners_BPCode",
                        column: x => x.BPCode,
                        principalTable: "BusinessPartners",
                        principalColumn: "CardCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bpfiscaltaxidcollections",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BPCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CNAECode = table.Column<int>(type: "int", nullable: true),
                    TaxId0 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId11 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddrType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId12 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId13 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AToRetrNFe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxId14 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bpfiscaltaxidcollections", x => new { x.BPCode, x.Address });
                    table.ForeignKey(
                        name: "FK_Bpfiscaltaxidcollections_BusinessPartners_BPCode",
                        column: x => x.BPCode,
                        principalTable: "BusinessPartners",
                        principalColumn: "CardCode",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bpaddresses");

            migrationBuilder.DropTable(
                name: "Bpfiscaltaxidcollections");

            migrationBuilder.DropTable(
                name: "BusinessPartners");
        }
    }
}
