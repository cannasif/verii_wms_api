using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBarcodeDefinitionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_BARCODE_DEFINITION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Format = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    EnableErpFallback = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    HintText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ErpWarehouseCode = table.Column<int>(type: "int", nullable: false),
                    ErpModule = table.Column<int>(type: "int", nullable: false),
                    ErpMovementType = table.Column<int>(type: "int", nullable: false),
                    BarcodeGroup = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "1"),
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
                    table.PrimaryKey("PK_RII_BARCODE_DEFINITION", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UX_RII_BARCODE_DEFINITION_BRANCH_MODULE",
                table: "RII_BARCODE_DEFINITION",
                columns: new[] { "BranchCode", "ModuleKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_BARCODE_DEFINITION");
        }
    }
}
