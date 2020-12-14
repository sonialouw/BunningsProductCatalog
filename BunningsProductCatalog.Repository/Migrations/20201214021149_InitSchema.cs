using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BunningsProductCatalog.Repository.Migrations
{
    public partial class InitSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyCode = table.Column<string>(maxLength: 10, nullable: false),
                    CompanyName = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProducts",
                columns: table => new
                {
                    CompanyProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(maxLength: 300, nullable: false),
                    ProductSku = table.Column<string>(maxLength: 100, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProducts", x => x.CompanyProductId);
                    table.ForeignKey(
                        name: "FK_CompanyProducts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(maxLength: 300, nullable: false),
                    SupplierCode = table.Column<string>(maxLength: 50, nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                    table.ForeignKey(
                        name: "FK_Suppliers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProductBarcodes",
                columns: table => new
                {
                    CompanyProductBarcodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(maxLength: 100, nullable: false),
                    CompanyProductId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false),
                    ModifiedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProductBarcodes", x => x.CompanyProductBarcodeId);
                    table.ForeignKey(
                        name: "FK_CompanyProductBarcodes_CompanyProducts_CompanyProductId",
                        column: x => x.CompanyProductId,
                        principalTable: "CompanyProducts",
                        principalColumn: "CompanyProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProductBarcodes_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyCode", "CompanyName" },
                values: new object[] { 1, "A", "Company A" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "CompanyCode", "CompanyName" },
                values: new object[] { 2, "B", "Company B" });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProductBarcodes_CompanyProductId",
                table: "CompanyProductBarcodes",
                column: "CompanyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProductBarcodes_SupplierId",
                table: "CompanyProductBarcodes",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProducts_CompanyId",
                table: "CompanyProducts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CompanyId",
                table: "Suppliers",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyProductBarcodes");

            migrationBuilder.DropTable(
                name: "CompanyProducts");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
