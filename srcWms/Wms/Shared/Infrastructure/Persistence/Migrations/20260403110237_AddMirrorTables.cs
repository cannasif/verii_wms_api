using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMirrorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_WMS_CUSTOMER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxOffice = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TcknNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    SalesRepCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    BusinessUnitCode = table.Column<short>(type: "smallint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsErpIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErpIntegrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "0"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WMS_CUSTOMER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_WMS_STOCK",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpStockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UreticiKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod1Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod2Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod3Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod4Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod5Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "0"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WMS_STOCK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_WMS_WAREHOUSE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseCode = table.Column<int>(type: "int", nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "0"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WMS_WAREHOUSE", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerCode",
                table: "RII_WMS_CUSTOMER",
                column: "CustomerCode",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerName",
                table: "RII_WMS_CUSTOMER",
                column: "CustomerName");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IsDeleted",
                table: "RII_WMS_CUSTOMER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ErpStockCode",
                table: "RII_WMS_STOCK",
                column: "ErpStockCode",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_StockName",
                table: "RII_WMS_STOCK",
                column: "StockName");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IsDeleted",
                table: "RII_WMS_STOCK",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_IsDeleted",
                table: "RII_WMS_WAREHOUSE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_WarehouseCode",
                table: "RII_WMS_WAREHOUSE",
                column: "WarehouseCode",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_WarehouseName",
                table: "RII_WMS_WAREHOUSE",
                column: "WarehouseName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_WMS_CUSTOMER");

            migrationBuilder.DropTable(
                name: "RII_WMS_STOCK");

            migrationBuilder.DropTable(
                name: "RII_WMS_WAREHOUSE");
        }
    }
}
