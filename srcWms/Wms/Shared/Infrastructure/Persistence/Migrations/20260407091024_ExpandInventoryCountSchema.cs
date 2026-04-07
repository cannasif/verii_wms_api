using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExpandInventoryCountSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "RII_IC_HEADER");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "RII_IC_HEADER",
                newName: "YapKod");

            migrationBuilder.AddColumn<long>(
                name: "AssignedRoleId",
                table: "RII_IC_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedTeamId",
                table: "RII_IC_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssignedUserId",
                table: "RII_IC_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountMode",
                table: "RII_IC_HEADER",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountType",
                table: "RII_IC_HEADER",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CountedLineCount",
                table: "RII_IC_HEADER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DifferenceLineCount",
                table: "RII_IC_HEADER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FreezeMode",
                table: "RII_IC_HEADER",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstCount",
                table: "RII_IC_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LineCount",
                table: "RII_IC_HEADER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndDate",
                table: "RII_IC_HEADER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDate",
                table: "RII_IC_HEADER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RackCode",
                table: "RII_IC_HEADER",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecountRequiredLineCount",
                table: "RII_IC_HEADER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresRecount",
                table: "RII_IC_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ScopeMode",
                table: "RII_IC_HEADER",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SequenceNo",
                table: "RII_IC_HEADER",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedDate",
                table: "RII_IC_HEADER",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "RII_IC_HEADER",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_IC_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_IC_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_IC_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RII_IC_SCOPE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: true),
                    ScopeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    WarehouseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YapKodId = table.Column<long>(type: "bigint", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RackCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CellCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_RII_IC_SCOPE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_SCOPE_RII_IC_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_IC_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    ScopeId = table.Column<long>(type: "bigint", nullable: true),
                    SequenceNo = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    WarehouseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RackCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CellCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKodId = table.Column<long>(type: "bigint", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LotNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExpectedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CountedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    DifferenceQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CountStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EntryCount = table.Column<int>(type: "int", nullable: false),
                    IsMatched = table.Column<bool>(type: "bit", nullable: false),
                    IsDifference = table.Column<bool>(type: "bit", nullable: false),
                    IsExtraStock = table.Column<bool>(type: "bit", nullable: false),
                    IsMissingStock = table.Column<bool>(type: "bit", nullable: false),
                    IsRecountRequired = table.Column<bool>(type: "bit", nullable: false),
                    FirstCountedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastCountedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalNote = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DifferenceReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_RII_IC_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_LINE_RII_IC_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_IC_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_IC_LINE_RII_IC_SCOPE_ScopeId",
                        column: x => x.ScopeId,
                        principalTable: "RII_IC_SCOPE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_ADJUSTMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    ExpectedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ApprovedCountedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DifferenceQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AdjustmentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErpReferenceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_RII_IC_ADJUSTMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_ADJUSTMENT_RII_IC_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_IC_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_IC_ADJUSTMENT_RII_IC_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_IC_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_COUNT_ENTRY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    EntryNo = table.Column<int>(type: "int", nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EnteredQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    WarehouseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RackCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CellCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    EnteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnteredByUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_RII_IC_COUNT_ENTRY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_COUNT_ENTRY_RII_IC_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_IC_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_IC_COUNT_ENTRY_RII_IC_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_IC_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IcHeader_AssignedUserId",
                table: "RII_IC_HEADER",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IcHeader_CountType",
                table: "RII_IC_HEADER",
                column: "CountType");

            migrationBuilder.CreateIndex(
                name: "IX_IcHeader_Status",
                table: "RII_IC_HEADER",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_IcHeader_StockId",
                table: "RII_IC_HEADER",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_IcAdjustment_HeaderId",
                table: "RII_IC_ADJUSTMENT",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_IcAdjustment_LineId",
                table: "RII_IC_ADJUSTMENT",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_IcAdjustment_Status",
                table: "RII_IC_ADJUSTMENT",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_IcCountEntry_EntryType",
                table: "RII_IC_COUNT_ENTRY",
                column: "EntryType");

            migrationBuilder.CreateIndex(
                name: "IX_IcCountEntry_HeaderId",
                table: "RII_IC_COUNT_ENTRY",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_IcCountEntry_LineId",
                table: "RII_IC_COUNT_ENTRY",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_IcLine_CountStatus",
                table: "RII_IC_LINE",
                column: "CountStatus");

            migrationBuilder.CreateIndex(
                name: "IX_IcLine_HeaderId",
                table: "RII_IC_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_IcLine_ScopeId",
                table: "RII_IC_LINE",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_IcLine_StockId",
                table: "RII_IC_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_IcScope_HeaderId",
                table: "RII_IC_SCOPE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_IcScope_ScopeType",
                table: "RII_IC_SCOPE",
                column: "ScopeType");

            migrationBuilder.CreateIndex(
                name: "IX_IcScope_StockId",
                table: "RII_IC_SCOPE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_IcScope_WarehouseId",
                table: "RII_IC_SCOPE",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_IC_ADJUSTMENT");

            migrationBuilder.DropTable(
                name: "RII_IC_COUNT_ENTRY");

            migrationBuilder.DropTable(
                name: "RII_IC_LINE");

            migrationBuilder.DropTable(
                name: "RII_IC_SCOPE");

            migrationBuilder.DropIndex(
                name: "IX_IcHeader_AssignedUserId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_IcHeader_CountType",
                table: "RII_IC_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_IcHeader_Status",
                table: "RII_IC_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_IcHeader_StockId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "AssignedRoleId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "AssignedTeamId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "CountMode",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "CountType",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "CountedLineCount",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "DifferenceLineCount",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "FreezeMode",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "IsFirstCount",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "LineCount",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "PlannedEndDate",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "PlannedStartDate",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "RackCode",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "RecountRequiredLineCount",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "RequiresRecount",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "ScopeMode",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "SequenceNo",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "StartedDate",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_IC_HEADER");

            migrationBuilder.RenameColumn(
                name: "YapKod",
                table: "RII_IC_HEADER",
                newName: "ProductCode");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "RII_IC_HEADER",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
