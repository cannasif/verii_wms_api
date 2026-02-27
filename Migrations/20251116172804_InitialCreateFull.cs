using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlatformPageGroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MenuHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    MenuLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformPageGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_GR_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ElectronicWaybill = table.Column<bool>(type: "bit", nullable: false),
                    ReturnCode = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OCRSource = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Description3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description4 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description5 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_GR_IMPORT_DOCUMENT",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    Base64 = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_IMPORT_DOCUMENT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrImportDocument_GrHeader",
                        column: x => x.HeaderId,
                        principalTable: "RII_GR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_GR_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GR_IMPORT_LINE_RII_GR_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_GR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_GR_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrLine_GrHeader",
                        column: x => x.HeaderId,
                        principalTable: "RII_GR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_GR_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    GrLineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_GR_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_GR_LINE_SERIAL_RII_GR_LINE_GrLineId",
                        column: x => x.GrLineId,
                        principalTable: "RII_GR_LINE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_GR_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GR_ROUTE_RII_GR_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_GR_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CellCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_IC_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_IC_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_IMPORT_LINE_RII_IC_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_IC_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OldQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    NewQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldWarehouse = table.Column<int>(type: "int", nullable: true),
                    NewWarehouse = table.Column<int>(type: "int", nullable: true),
                    OldCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NewCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_IC_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_ROUTE_RII_IC_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_IC_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_IC_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_TERMINAL_LINE_RII_IC_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_IC_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_MOBIL_PAGE_GROUP",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MenuHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    MenuLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_MOBIL_PAGE_GROUP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_MOBIL_USER_GROUP_MATCH",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_MOBIL_USER_GROUP_MATCH", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_MOBIL_USER_PAGE_GROUP_MATCH",
                columns: table => new
                {
                    MobilePageGroupsId = table.Column<long>(type: "bigint", nullable: false),
                    UserGroupMatchesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_MOBIL_USER_PAGE_GROUP_MATCH", x => new { x.MobilePageGroupsId, x.UserGroupMatchesId });
                    table.ForeignKey(
                        name: "FK_RII_MOBIL_USER_PAGE_GROUP_MATCH_RII_MOBIL_PAGE_GROUP_MobilePageGroupsId",
                        column: x => x.MobilePageGroupsId,
                        principalTable: "RII_MOBIL_PAGE_GROUP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_MOBIL_USER_PAGE_GROUP_MATCH_RII_MOBIL_USER_GROUP_MATCH_UserGroupMatchesId",
                        column: x => x.UserGroupMatchesId,
                        principalTable: "RII_MOBIL_USER_GROUP_MATCH",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_MOBILMENU_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_MOBILMENU_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_MOBILMENU_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_MOBILMENU_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_MOBILMENU_LINE_RII_MOBILMENU_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_MOBILMENU_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_PLATFORM_SIDEBARMENU_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DarkColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShadowColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DarkShadowColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DarkTextColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PLATFORM_SIDEBARMENU_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_PLATFORM_SIDEBARMENU_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    Page = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PLATFORM_SIDEBARMENU_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PLATFORM_SIDEBARMENU_LINE_RII_PLATFORM_SIDEBARMENU_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PLATFORM_SIDEBARMENU_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_PLATFORM_USER_GROUP_MATCH",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GroupsId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PLATFORM_USER_GROUP_MATCH", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PLATFORM_USER_GROUP_MATCH_PlatformPageGroup_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "PlatformPageGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PT_IMPORT_LINE_RII_PT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PT_LINE_RII_PT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PT_LINE_SERIAL_RII_PT_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_PT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PT_ROUTE_RII_PT_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_PT_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    PtHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PT_TERMINAL_LINE_RII_PT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PT_TERMINAL_LINE_RII_PT_HEADER_PtHeaderId",
                        column: x => x.PtHeaderId,
                        principalTable: "RII_PT_HEADER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SIT_IMPORT_LINE_RII_SIT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SIT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SIT_LINE_RII_SIT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SIT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SIT_LINE_SERIAL_RII_SIT_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_SIT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SIT_ROUTE_RII_SIT_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_SIT_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    SitHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SIT_TERMINAL_LINE_RII_SIT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SIT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SIT_TERMINAL_LINE_RII_SIT_HEADER_SitHeaderId",
                        column: x => x.SitHeaderId,
                        principalTable: "RII_SIT_HEADER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_HEADER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SRT_IMPORT_LINE_RII_SRT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SRT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SRT_LINE_RII_SRT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SRT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SRT_LINE_SERIAL_RII_SRT_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_SRT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SRT_ROUTE_RII_SRT_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_SRT_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    SrtHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SRT_TERMINAL_LINE_RII_SRT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SRT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SRT_TERMINAL_LINE_RII_SRT_HEADER_SrtHeaderId",
                        column: x => x.SrtHeaderId,
                        principalTable: "RII_SRT_HEADER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_AUTHORITY",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_AUTHORITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_USERS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false, defaultValue: 1L),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USERS_RII_USER_AUTHORITY_RoleId",
                        column: x => x.RoleId,
                        principalTable: "RII_USER_AUTHORITY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_USER_SESSION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeviceInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_USER_SESSION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_SESSION_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InboundType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_HEADER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_HEADER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_HEADER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutboundType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AccountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_HEADER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_HEADER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_HEADER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ElectronicWaybill = table.Column<bool>(type: "bit", nullable: false),
                    ShipmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_HEADER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_HEADER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_HEADER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_RII_WI_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WI_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    WiHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_TERMINAL_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_TERMINAL_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_TERMINAL_LINE_RII_USERS_TerminalUserId",
                        column: x => x.TerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WI_TERMINAL_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_TERMINAL_LINE_RII_WI_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WI_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WI_TERMINAL_LINE_RII_WI_HEADER_WiHeaderId",
                        column: x => x.WiHeaderId,
                        principalTable: "RII_WI_HEADER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_RII_WO_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WO_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    WoHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_TERMINAL_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_TERMINAL_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_TERMINAL_LINE_RII_USERS_TerminalUserId",
                        column: x => x.TerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WO_TERMINAL_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_TERMINAL_LINE_RII_WO_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WO_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WO_TERMINAL_LINE_RII_WO_HEADER_WoHeaderId",
                        column: x => x.WoHeaderId,
                        principalTable: "RII_WO_HEADER",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderLineNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_RII_WT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_TERMINAL_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_TERMINAL_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_TERMINAL_LINE_RII_USERS_TerminalUserId",
                        column: x => x.TerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_WT_TERMINAL_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_TERMINAL_LINE_RII_WT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_IMPORT_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_IMPORT_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_IMPORT_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_IMPORT_LINE_RII_WI_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WI_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WI_IMPORT_LINE_RII_WI_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_WI_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_SERIAL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_SERIAL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_SERIAL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_LINE_SERIAL_RII_WI_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_WI_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_IMPORT_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_IMPORT_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_IMPORT_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_IMPORT_LINE_RII_WO_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WO_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WO_IMPORT_LINE_RII_WO_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_WO_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_SERIAL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_SERIAL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_SERIAL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_LINE_SERIAL_RII_WO_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_WO_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_IMPORT_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_IMPORT_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_IMPORT_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_IMPORT_LINE_RII_WT_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_WT_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_WT_IMPORT_LINE_RII_WT_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_WT_LINE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_SERIAL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_SERIAL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_SERIAL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_LINE_SERIAL_RII_WT_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_WT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_ROUTE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_ROUTE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_ROUTE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_ROUTE_RII_WI_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_WI_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    WoImportLineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_ROUTE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_ROUTE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_ROUTE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_ROUTE_RII_WO_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_WO_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_WO_ROUTE_RII_WO_IMPORT_LINE_WoImportLineId",
                        column: x => x.WoImportLineId,
                        principalTable: "RII_WO_IMPORT_LINE",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    RouteCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_ROUTE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_ROUTE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_ROUTE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_ROUTE_RII_WT_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_WT_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RII_MOBILMENU_HEADER",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Icon", "IsOpen", "MenuId", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "cube-outline", false, "mal-kabul", "malKabul", null, null },
                    { 2L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "car-outline", false, "sevkiyat", "sevkiyat", null, null },
                    { 3L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "swap-horizontal-outline", false, "transfer", "transfer", null, null },
                    { 4L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "calculator-outline", false, "sayim", "sayim", null, null },
                    { 5L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "grid-outline", false, "hucre-takibi", "hucreTakibi", null, null },
                    { 6L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "construct-outline", false, "uretim", "uretim", null, null },
                    { 7L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "cube", false, "paketleme", "paketleme", null, null },
                    { 8L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "barcode-outline", false, "sesli-komut", "sesliKomut", null, null },
                    { 9L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "information-circle-outline", false, "genel-bilgi", "genelBilgi", null, null }
                });

            migrationBuilder.InsertData(
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                columns: new[] { "Id", "Color", "CreatedBy", "CreatedDate", "DarkColor", "DarkShadowColor", "DarkTextColor", "DeletedBy", "DeletedDate", "Icon", "MenuKey", "RoleLevel", "ShadowColor", "TextColor", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, "blue-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "blue-700", "blue-500", "blue-400", null, null, "HiOutlineCube", "malKabul", 2, "blue-300", "blue-600", "sidebar.malKabul.title", null, null },
                    { 2L, "purple-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "purple-700", "purple-500", "purple-400", null, null, "HiOutlineTruck", "sevkiyat", 2, "purple-300", "purple-600", "sidebar.sevkiyat.title", null, null },
                    { 3L, "cyan-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "cyan-700", "cyan-500", "cyan-400", null, null, "HiOutlineRefresh", "transfer", 2, "cyan-300", "cyan-600", "sidebar.transfer.title", null, null },
                    { 4L, "orange-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "orange-700", "orange-500", "orange-400", null, null, "HiOutlineCalculator", "sayim", 2, "orange-300", "orange-600", "sidebar.sayim.title", null, null },
                    { 5L, "indigo-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "indigo-700", "indigo-500", "indigo-400", null, null, "HiOutlineViewGrid", "hucreTakibi", 2, "indigo-300", "indigo-600", "sidebar.hucreTakibi.title", null, null },
                    { 6L, "yellow-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "yellow-700", "yellow-500", "yellow-400", null, null, "HiOutlineCollection", "uretim", 2, "yellow-300", "yellow-600", "sidebar.uretim.title", null, null },
                    { 7L, "red-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "red-700", "red-500", "red-400", null, null, "HiOutlineCollection", "paketleme", 2, "red-300", "red-600", "sidebar.paketleme.title", null, null },
                    { 8L, "green-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "green-700", "green-500", "green-400", null, null, "HiOutlineCog", "yonetim", 3, "green-300", "green-600", "sidebar.yonetim.title", null, null },
                    { 9L, "teal-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "teal-700", "teal-500", "teal-400", null, null, "RiSettingsFill", "parametreler", 2, "teal-300", "teal-600", "sidebar.parametreler.title", null, null },
                    { 10L, "fuchsia-100", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "fuchsia-700", "fuchsia-500", "fuchsia-400", null, null, "LuFileChartLine", "raporlar", 2, "fuchsia-300", "fuchsia-600", "sidebar.raporlar.title", null, null }
                });

            migrationBuilder.InsertData(
                table: "RII_USER_AUTHORITY",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "user", null, null },
                    { 2L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin", null, null },
                    { 3L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "superadmin", null, null }
                });

            migrationBuilder.InsertData(
                table: "RII_MOBILMENU_LINE",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Description", "HeaderId", "Icon", "ItemId", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "iadeGirisiDesc", 1L, "return-up-back-outline", "iade-girisi", "iadeGirisi", null, null },
                    { 2L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "irsaliyeFaturaDesc", 1L, "document-text-outline", "irsaliye-fatura", "irsaliyeFatura", null, null },
                    { 3L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sevkiyatEmriDesc", 2L, "send-outline", "sevkiyat-emri", "sevkiyatEmri", null, null },
                    { 4L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sevkiyatKontrolDesc", 2L, "send-outline", "sevkiyat-kontrol", "sevkiyatKontrol", null, null },
                    { 5L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "depoTransferiDesc", 3L, "archive-outline", "depo-transferi", "depoTransferi", null, null },
                    { 6L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "ambarGirisDesc", 3L, "enter-outline", "ambar-giris", "ambarGiris", null, null },
                    { 7L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "ambarCikisDesc", 3L, "exit-outline", "ambar-cikis", "ambarCikis", null, null },
                    { 8L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "planliDepoTransferiDesc", 3L, "calendar-outline", "planli-depo-transferi", "planliDepoTransferi", null, null },
                    { 9L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "planliAmbarCikisDesc", 3L, "calendar-clear-outline", "planli-ambar-cikis", "planliAmbarCikis", null, null },
                    { 10L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sayimGirisiDesc", 4L, "list-outline", "sayim-girisi", "sayimGirisi", null, null },
                    { 11L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "hucreBilgisiDesc", 5L, "move-outline", "hucre-transferi", "hucreBilgisi", null, null },
                    { 12L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "hucrelerArasiTransferDesc", 5L, "swap-vertical-outline", "hucreler-arasi-transfer", "hucrelerArasiTransfer", null, null },
                    { 13L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "planliHucreTransferiDesc", 5L, "calendar-outline", "planli-hucre-transferi", "planliHucreTransferi", null, null },
                    { 14L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "uretimSonuKaydiDesc", 6L, "checkmark-done-outline", "uretim-sonu-kaydi", "uretimSonuKaydi", null, null },
                    { 15L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "kioskDesc", 6L, "hammer-outline", "kiosk", "kiosk", null, null },
                    { 16L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "paketlemeGirisiDesc", 7L, "gift-outline", "paketleme-girisi", "paketlemeGirisi", null, null },
                    { 17L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "paketlemeIslemleriDesc", 7L, "layers-outline", "paketleme-islemleri", "paketlemeIslemleri", null, null },
                    { 18L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sesliKomutTestDesc", 8L, "repeat-outline", "sesli-komut-test", "sesliKomutTest", null, null },
                    { 19L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "stokDetayEkraniDesc", 9L, "analytics-outline", "stok-detay-ekrani", "stokDetayEkrani", null, null }
                });

            migrationBuilder.InsertData(
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DeletedBy", "DeletedDate", "Description", "HeaderId", "Icon", "Page", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.malKabul.completedListDesc", 1L, "HiOutlineDocumentText", "tamamlananMalKabulListesi", "sidebar.malKabul.completedList", null, null },
                    { 2L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.sevkiyat.createDesc", 2L, "HiOutlineTruck", "sevkiyatOlustur", "sidebar.sevkiyat.create", null, null },
                    { 3L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.sevkiyat.completedListDesc", 2L, "HiOutlineDocumentText", "tamamlananSevkiyatListesi", "sidebar.sevkiyat.completedList", null, null },
                    { 4L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.transfer.createInterWarehouseDesc", 3L, "HiOutlineRefresh", "depolarArasiTransferOlustur", "sidebar.transfer.createInterWarehouse", null, null },
                    { 5L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.transfer.orderBasedDesc", 3L, "HiOutlineDocumentText", "sipariseIstinadenDepolarArasiTransfer", "sidebar.transfer.orderBased", null, null },
                    { 6L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.transfer.completedDesc", 3L, "HiOutlineDocumentText", "tamamlanmisTransferEmirleri", "sidebar.transfer.completed", null, null },
                    { 7L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.transfer.productionBasedDesc", 3L, "HiOutlineCollection", "uretimeTransfer", "sidebar.transfer.productionBased", null, null },
                    { 8L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.transfer.warehouseExitDesc", 3L, "HiOutlineArrowLeft", "ambarCikisOlustur", "sidebar.transfer.warehouseExit", null, null },
                    { 9L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.sayim.createOrderDesc", 4L, "HiOutlineCalculator", "sayimEmriOlustur", "sidebar.sayim.createOrder", null, null },
                    { 10L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.sayim.completedListDesc", 4L, "HiOutlineDocumentText", "tamamlananSayimListesi", "sidebar.sayim.completedList", null, null },
                    { 11L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.hucreTakibi.createOrderDesc", 5L, "HiOutlineViewGrid", "hucreEmriOlustur", "sidebar.hucreTakibi.createOrder", null, null },
                    { 12L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.uretim.completedListDesc", 6L, "HiOutlineDocumentText", "tamamlananUretimListesi", "sidebar.uretim.completedList", null, null },
                    { 13L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.paketleme.completedListDesc", 7L, "HiOutlineDocumentText", "tamamlananPaketlemeListesi", "sidebar.paketleme.completedList", null, null },
                    { 14L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.kullanici.islemleriDesc", 8L, "HiOutlineUsers", "kullaniciIslemleri", "sidebar.kullanici.islemleri", null, null },
                    { 15L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.platformGrup.islemleriDesc", 8L, "HiOutlineUserGroup", "platformGrupIslemleri", "sidebar.platformGrup.islemleri", null, null },
                    { 16L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.platformKullaniciGrup.islemleriDesc", 8L, "HiOutlineUserGroup", "platformKullaniciGrupEslemeIslemleri", "sidebar.platformKullaniciGrup.islemleri", null, null },
                    { 17L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.mobilGrup.islemleriDesc", 8L, "HiOutlineUserGroup", "mobilGrupIslemleri", "sidebar.mobilGrup.islemleri", null, null },
                    { 18L, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "sidebar.mobilKullaniciGrup.islemleriDesc", 8L, "HiOutlineUserGroup", "mobilKullaniciGrupEslemeIslemleri", "sidebar.mobilKullaniciGrup.islemleri", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_CreatedBy",
                table: "PlatformPageGroup",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_DeletedBy",
                table: "PlatformPageGroup",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_GroupCode",
                table: "PlatformPageGroup",
                column: "GroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_IsDeleted",
                table: "PlatformPageGroup",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_MenuHeaderId",
                table: "PlatformPageGroup",
                column: "MenuHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_MenuLineId",
                table: "PlatformPageGroup",
                column: "MenuLineId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformPageGroup_UpdatedBy",
                table: "PlatformPageGroup",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrHeader_BranchCode",
                table: "RII_GR_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_GrHeader_CustomerCode",
                table: "RII_GR_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_GrHeader_PlannedDate",
                table: "RII_GR_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_HEADER_CreatedBy",
                table: "RII_GR_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_HEADER_DeletedBy",
                table: "RII_GR_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_HEADER_UpdatedBy",
                table: "RII_GR_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrImportDocument_HeaderId",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GrImportDocument_IsDeleted",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_DOCUMENT_CreatedBy",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_DOCUMENT_DeletedBy",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_DOCUMENT_UpdatedBy",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_LINE_CreatedBy",
                table: "RII_GR_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_LINE_DeletedBy",
                table: "RII_GR_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_LINE_HeaderId",
                table: "RII_GR_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_LINE_LineId",
                table: "RII_GR_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_IMPORT_LINE_UpdatedBy",
                table: "RII_GR_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrLine_HeaderId",
                table: "RII_GR_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GrLine_IsDeleted",
                table: "RII_GR_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GrLine_StockCode",
                table: "RII_GR_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_CreatedBy",
                table: "RII_GR_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_DeletedBy",
                table: "RII_GR_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_UpdatedBy",
                table: "RII_GR_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GrLineSerial_ImportLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_GrLineSerial_IsDeleted",
                table: "RII_GR_LINE_SERIAL",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_CreatedBy",
                table: "RII_GR_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_DeletedBy",
                table: "RII_GR_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_UpdatedBy",
                table: "RII_GR_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_ROUTE_CreatedBy",
                table: "RII_GR_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_ROUTE_DeletedBy",
                table: "RII_GR_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_ROUTE_ImportLineId",
                table: "RII_GR_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_ROUTE_UpdatedBy",
                table: "RII_GR_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_HEADER_CreatedBy",
                table: "RII_IC_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_HEADER_DeletedBy",
                table: "RII_IC_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_HEADER_UpdatedBy",
                table: "RII_IC_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_IMPORT_LINE_CreatedBy",
                table: "RII_IC_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_IMPORT_LINE_DeletedBy",
                table: "RII_IC_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_IMPORT_LINE_HeaderId",
                table: "RII_IC_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_IMPORT_LINE_UpdatedBy",
                table: "RII_IC_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_ROUTE_CreatedBy",
                table: "RII_IC_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_ROUTE_DeletedBy",
                table: "RII_IC_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_ROUTE_ImportLineId",
                table: "RII_IC_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_ROUTE_UpdatedBy",
                table: "RII_IC_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_TERMINAL_LINE_CreatedBy",
                table: "RII_IC_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_TERMINAL_LINE_DeletedBy",
                table: "RII_IC_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_TERMINAL_LINE_HeaderId",
                table: "RII_IC_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_TERMINAL_LINE_TerminalUserId",
                table: "RII_IC_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_TERMINAL_LINE_UpdatedBy",
                table: "RII_IC_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_PAGE_GROUP_CreatedBy",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_PAGE_GROUP_DeletedBy",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_PAGE_GROUP_MenuHeaderId",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "MenuHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_PAGE_GROUP_MenuLineId",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "MenuLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_PAGE_GROUP_UpdatedBy",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_USER_GROUP_MATCH_CreatedBy",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_USER_GROUP_MATCH_DeletedBy",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_USER_GROUP_MATCH_UpdatedBy",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_USER_GROUP_MATCH_UserId",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBIL_USER_PAGE_GROUP_MATCH_UserGroupMatchesId",
                table: "RII_MOBIL_USER_PAGE_GROUP_MATCH",
                column: "UserGroupMatchesId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_HEADER_CreatedBy",
                table: "RII_MOBILMENU_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_HEADER_DeletedBy",
                table: "RII_MOBILMENU_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_HEADER_UpdatedBy",
                table: "RII_MOBILMENU_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_LINE_CreatedBy",
                table: "RII_MOBILMENU_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_LINE_DeletedBy",
                table: "RII_MOBILMENU_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_LINE_HeaderId",
                table: "RII_MOBILMENU_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_MOBILMENU_LINE_UpdatedBy",
                table: "RII_MOBILMENU_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_SIDEBARMENU_HEADER_CreatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_SIDEBARMENU_HEADER_DeletedBy",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_SIDEBARMENU_HEADER_UpdatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SidebarmenuHeader_IsDeleted",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SidebarmenuHeader_MenuKey",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "MenuKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SidebarmenuHeader_RoleLevel",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "RoleLevel");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_SIDEBARMENU_LINE_CreatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_SIDEBARMENU_LINE_DeletedBy",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_SIDEBARMENU_LINE_UpdatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SidebarmenuLine_HeaderId",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SidebarmenuLine_IsDeleted",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SidebarmenuLine_Page",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "Page",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserGroupMatch_GroupCode",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "GroupCode");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserGroupMatch_IsDeleted",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserGroupMatch_UserId",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformUserGroupMatch_UserId_GroupCode",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                columns: new[] { "UserId", "GroupCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_USER_GROUP_MATCH_CreatedBy",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_USER_GROUP_MATCH_DeletedBy",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_USER_GROUP_MATCH_GroupsId",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PLATFORM_USER_GROUP_MATCH_UpdatedBy",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_BranchCode",
                table: "RII_PT_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_CustomerCode",
                table: "RII_PT_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_IsDeleted",
                table: "RII_PT_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_PlannedDate",
                table: "RII_PT_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_ProjectCode",
                table: "RII_PT_HEADER",
                column: "ProjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_YearCode",
                table: "RII_PT_HEADER",
                column: "YearCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_HEADER_CreatedBy",
                table: "RII_PT_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_HEADER_DeletedBy",
                table: "RII_PT_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_HEADER_UpdatedBy",
                table: "RII_PT_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PtImportLine_HeaderId",
                table: "RII_PT_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PtImportLine_IsDeleted",
                table: "RII_PT_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PtImportLine_LineId",
                table: "RII_PT_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_PtImportLine_StockCode",
                table: "RII_PT_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_IMPORT_LINE_CreatedBy",
                table: "RII_PT_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_IMPORT_LINE_DeletedBy",
                table: "RII_PT_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_IMPORT_LINE_UpdatedBy",
                table: "RII_PT_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PtLine_ErpOrderNo",
                table: "RII_PT_LINE",
                column: "ErpOrderNo");

            migrationBuilder.CreateIndex(
                name: "IX_PtLine_HeaderId",
                table: "RII_PT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PtLine_IsDeleted",
                table: "RII_PT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PtLine_StockCode",
                table: "RII_PT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_CreatedBy",
                table: "RII_PT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_DeletedBy",
                table: "RII_PT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_UpdatedBy",
                table: "RII_PT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_SERIAL_CreatedBy",
                table: "RII_PT_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_SERIAL_DeletedBy",
                table: "RII_PT_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_SERIAL_LineId",
                table: "RII_PT_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_LINE_SERIAL_UpdatedBy",
                table: "RII_PT_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PtRoute_ImportLineId",
                table: "RII_PT_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_PtRoute_IsDeleted",
                table: "RII_PT_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PtRoute_SerialNo",
                table: "RII_PT_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_PtRoute_SourceWarehouse",
                table: "RII_PT_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_PtRoute_TargetWarehouse",
                table: "RII_PT_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_ROUTE_CreatedBy",
                table: "RII_PT_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_ROUTE_DeletedBy",
                table: "RII_PT_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_ROUTE_UpdatedBy",
                table: "RII_PT_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PtTerminalLine_HeaderId",
                table: "RII_PT_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PtTerminalLine_IsDeleted",
                table: "RII_PT_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PtTerminalLine_TerminalUserId",
                table: "RII_PT_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_TERMINAL_LINE_CreatedBy",
                table: "RII_PT_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_TERMINAL_LINE_DeletedBy",
                table: "RII_PT_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_TERMINAL_LINE_PtHeaderId",
                table: "RII_PT_TERMINAL_LINE",
                column: "PtHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_TERMINAL_LINE_UpdatedBy",
                table: "RII_PT_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_HEADER_CreatedBy",
                table: "RII_SIT_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_HEADER_DeletedBy",
                table: "RII_SIT_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_HEADER_UpdatedBy",
                table: "RII_SIT_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_BranchCode",
                table: "RII_SIT_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_CustomerCode",
                table: "RII_SIT_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_IsDeleted",
                table: "RII_SIT_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_PlannedDate",
                table: "RII_SIT_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_ProjectCode",
                table: "RII_SIT_HEADER",
                column: "ProjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_YearCode",
                table: "RII_SIT_HEADER",
                column: "YearCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_IMPORT_LINE_CreatedBy",
                table: "RII_SIT_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_IMPORT_LINE_DeletedBy",
                table: "RII_SIT_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_IMPORT_LINE_UpdatedBy",
                table: "RII_SIT_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SitImportLine_HeaderId",
                table: "RII_SIT_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SitImportLine_IsDeleted",
                table: "RII_SIT_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SitImportLine_LineId",
                table: "RII_SIT_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_SitImportLine_StockCode",
                table: "RII_SIT_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_CreatedBy",
                table: "RII_SIT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_DeletedBy",
                table: "RII_SIT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_UpdatedBy",
                table: "RII_SIT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SitLine_ErpOrderNo",
                table: "RII_SIT_LINE",
                column: "ErpOrderNo");

            migrationBuilder.CreateIndex(
                name: "IX_SitLine_HeaderId",
                table: "RII_SIT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SitLine_IsDeleted",
                table: "RII_SIT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SitLine_StockCode",
                table: "RII_SIT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_SERIAL_CreatedBy",
                table: "RII_SIT_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_SERIAL_DeletedBy",
                table: "RII_SIT_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_SERIAL_LineId",
                table: "RII_SIT_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_LINE_SERIAL_UpdatedBy",
                table: "RII_SIT_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_ROUTE_CreatedBy",
                table: "RII_SIT_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_ROUTE_DeletedBy",
                table: "RII_SIT_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_ROUTE_UpdatedBy",
                table: "RII_SIT_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SitRoute_ImportLineId",
                table: "RII_SIT_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_SitRoute_IsDeleted",
                table: "RII_SIT_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SitRoute_SerialNo",
                table: "RII_SIT_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_SitRoute_SourceWarehouse",
                table: "RII_SIT_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_SitRoute_TargetWarehouse",
                table: "RII_SIT_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_TERMINAL_LINE_CreatedBy",
                table: "RII_SIT_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_TERMINAL_LINE_DeletedBy",
                table: "RII_SIT_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_TERMINAL_LINE_SitHeaderId",
                table: "RII_SIT_TERMINAL_LINE",
                column: "SitHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_TERMINAL_LINE_UpdatedBy",
                table: "RII_SIT_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SitTerminalLine_HeaderId",
                table: "RII_SIT_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SitTerminalLine_IsDeleted",
                table: "RII_SIT_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SitTerminalLine_TerminalUserId",
                table: "RII_SIT_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_HEADER_CreatedBy",
                table: "RII_SRT_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_HEADER_DeletedBy",
                table: "RII_SRT_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_HEADER_UpdatedBy",
                table: "RII_SRT_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SrtHeader_BranchCode",
                table: "RII_SRT_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_SrtHeader_IsDeleted",
                table: "RII_SRT_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SrtHeader_PlannedDate",
                table: "RII_SRT_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_IMPORT_LINE_CreatedBy",
                table: "RII_SRT_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_IMPORT_LINE_DeletedBy",
                table: "RII_SRT_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_IMPORT_LINE_UpdatedBy",
                table: "RII_SRT_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SrtImportLine_HeaderId",
                table: "RII_SRT_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtImportLine_IsDeleted",
                table: "RII_SRT_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SrtImportLine_LineId",
                table: "RII_SRT_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtImportLine_StockCode",
                table: "RII_SRT_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_CreatedBy",
                table: "RII_SRT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_DeletedBy",
                table: "RII_SRT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_UpdatedBy",
                table: "RII_SRT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SrtLine_ErpOrderNo",
                table: "RII_SRT_LINE",
                column: "ErpOrderNo");

            migrationBuilder.CreateIndex(
                name: "IX_SrtLine_HeaderId",
                table: "RII_SRT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtLine_IsDeleted",
                table: "RII_SRT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SrtLine_StockCode",
                table: "RII_SRT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_SERIAL_CreatedBy",
                table: "RII_SRT_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_SERIAL_DeletedBy",
                table: "RII_SRT_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_SERIAL_LineId",
                table: "RII_SRT_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_LINE_SERIAL_UpdatedBy",
                table: "RII_SRT_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_ROUTE_CreatedBy",
                table: "RII_SRT_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_ROUTE_DeletedBy",
                table: "RII_SRT_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_ROUTE_UpdatedBy",
                table: "RII_SRT_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SrtRoute_ImportLineId",
                table: "RII_SRT_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtRoute_IsDeleted",
                table: "RII_SRT_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SrtRoute_SerialNo",
                table: "RII_SRT_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_SrtRoute_SourceWarehouse",
                table: "RII_SRT_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_SrtRoute_TargetWarehouse",
                table: "RII_SRT_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_TERMINAL_LINE_CreatedBy",
                table: "RII_SRT_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_TERMINAL_LINE_DeletedBy",
                table: "RII_SRT_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_TERMINAL_LINE_SrtHeaderId",
                table: "RII_SRT_TERMINAL_LINE",
                column: "SrtHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_TERMINAL_LINE_UpdatedBy",
                table: "RII_SRT_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SrtTerminalLine_HeaderId",
                table: "RII_SRT_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtTerminalLine_IsDeleted",
                table: "RII_SRT_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_SrtTerminalLine_TerminalUserId",
                table: "RII_SRT_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_AUTHORITY_CreatedBy",
                table: "RII_USER_AUTHORITY",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_AUTHORITY_DeletedBy",
                table: "RII_USER_AUTHORITY",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_AUTHORITY_UpdatedBy",
                table: "RII_USER_AUTHORITY",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_CreatedBy",
                table: "RII_USER_SESSION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_DeletedBy",
                table: "RII_USER_SESSION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_SessionId",
                table: "RII_USER_SESSION",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_UpdatedBy",
                table: "RII_USER_SESSION",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_UserId",
                table: "RII_USER_SESSION",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_SESSION_UserId_RevokedAt",
                table: "RII_USER_SESSION",
                columns: new[] { "UserId", "RevokedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_CreatedBy",
                table: "RII_USERS",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_DeletedBy",
                table: "RII_USERS",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_Email",
                table: "RII_USERS",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_RoleId",
                table: "RII_USERS",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_UpdatedBy",
                table: "RII_USERS",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USERS_Username",
                table: "RII_USERS",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_HEADER_CreatedBy",
                table: "RII_WI_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_HEADER_DeletedBy",
                table: "RII_WI_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_HEADER_UpdatedBy",
                table: "RII_WI_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_AccountCode",
                table: "RII_WI_HEADER",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_BranchCode",
                table: "RII_WI_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_CustomerCode",
                table: "RII_WI_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_IsDeleted",
                table: "RII_WI_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_PlannedDate",
                table: "RII_WI_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_YearCode",
                table: "RII_WI_HEADER",
                column: "YearCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_IMPORT_LINE_CreatedBy",
                table: "RII_WI_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_IMPORT_LINE_DeletedBy",
                table: "RII_WI_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_IMPORT_LINE_UpdatedBy",
                table: "RII_WI_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WiImportLine_HeaderId",
                table: "RII_WI_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WiImportLine_IsDeleted",
                table: "RII_WI_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WiImportLine_LineId",
                table: "RII_WI_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_WiImportLine_StockCode",
                table: "RII_WI_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_CreatedBy",
                table: "RII_WI_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_DeletedBy",
                table: "RII_WI_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_UpdatedBy",
                table: "RII_WI_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WiLine_ErpOrderNo",
                table: "RII_WI_LINE",
                column: "ErpOrderNo");

            migrationBuilder.CreateIndex(
                name: "IX_WiLine_HeaderId",
                table: "RII_WI_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WiLine_IsDeleted",
                table: "RII_WI_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WiLine_StockCode",
                table: "RII_WI_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_SERIAL_CreatedBy",
                table: "RII_WI_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_SERIAL_DeletedBy",
                table: "RII_WI_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_SERIAL_LineId",
                table: "RII_WI_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_LINE_SERIAL_UpdatedBy",
                table: "RII_WI_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_ROUTE_CreatedBy",
                table: "RII_WI_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_ROUTE_DeletedBy",
                table: "RII_WI_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_ROUTE_UpdatedBy",
                table: "RII_WI_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WiRoute_ImportLineId",
                table: "RII_WI_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_WiRoute_IsDeleted",
                table: "RII_WI_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WiRoute_SerialNo",
                table: "RII_WI_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_WiRoute_SourceWarehouse",
                table: "RII_WI_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_WiRoute_TargetWarehouse",
                table: "RII_WI_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_TERMINAL_LINE_CreatedBy",
                table: "RII_WI_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_TERMINAL_LINE_DeletedBy",
                table: "RII_WI_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_TERMINAL_LINE_UpdatedBy",
                table: "RII_WI_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_TERMINAL_LINE_WiHeaderId",
                table: "RII_WI_TERMINAL_LINE",
                column: "WiHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WiTerminalLine_HeaderId",
                table: "RII_WI_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WiTerminalLine_IsDeleted",
                table: "RII_WI_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WiTerminalLine_TerminalUserId",
                table: "RII_WI_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_HEADER_CreatedBy",
                table: "RII_WO_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_HEADER_DeletedBy",
                table: "RII_WO_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_HEADER_UpdatedBy",
                table: "RII_WO_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_AccountCode",
                table: "RII_WO_HEADER",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_BranchCode",
                table: "RII_WO_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_CustomerCode",
                table: "RII_WO_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_IsDeleted",
                table: "RII_WO_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_PlannedDate",
                table: "RII_WO_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_YearCode",
                table: "RII_WO_HEADER",
                column: "YearCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_IMPORT_LINE_CreatedBy",
                table: "RII_WO_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_IMPORT_LINE_DeletedBy",
                table: "RII_WO_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_IMPORT_LINE_UpdatedBy",
                table: "RII_WO_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WoImportLine_HeaderId",
                table: "RII_WO_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WoImportLine_IsDeleted",
                table: "RII_WO_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WoImportLine_LineId",
                table: "RII_WO_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_WoImportLine_StockCode",
                table: "RII_WO_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_CreatedBy",
                table: "RII_WO_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_DeletedBy",
                table: "RII_WO_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_UpdatedBy",
                table: "RII_WO_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WoLine_ErpOrderNo",
                table: "RII_WO_LINE",
                column: "ErpOrderNo");

            migrationBuilder.CreateIndex(
                name: "IX_WoLine_HeaderId",
                table: "RII_WO_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WoLine_IsDeleted",
                table: "RII_WO_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WoLine_StockCode",
                table: "RII_WO_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_SERIAL_CreatedBy",
                table: "RII_WO_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_SERIAL_DeletedBy",
                table: "RII_WO_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_SERIAL_LineId",
                table: "RII_WO_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_LINE_SERIAL_UpdatedBy",
                table: "RII_WO_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_ROUTE_CreatedBy",
                table: "RII_WO_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_ROUTE_DeletedBy",
                table: "RII_WO_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_ROUTE_UpdatedBy",
                table: "RII_WO_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_ROUTE_WoImportLineId",
                table: "RII_WO_ROUTE",
                column: "WoImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_WoRoute_ImportLineId",
                table: "RII_WO_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_WoRoute_IsDeleted",
                table: "RII_WO_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WoRoute_SerialNo",
                table: "RII_WO_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_WoRoute_SourceWarehouse",
                table: "RII_WO_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_WoRoute_TargetWarehouse",
                table: "RII_WO_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_TERMINAL_LINE_CreatedBy",
                table: "RII_WO_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_TERMINAL_LINE_DeletedBy",
                table: "RII_WO_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_TERMINAL_LINE_UpdatedBy",
                table: "RII_WO_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_TERMINAL_LINE_WoHeaderId",
                table: "RII_WO_TERMINAL_LINE",
                column: "WoHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WoTerminalLine_HeaderId",
                table: "RII_WO_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WoTerminalLine_IsDeleted",
                table: "RII_WO_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_WoTerminalLine_TerminalUserId",
                table: "RII_WO_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_HEADER_CreatedBy",
                table: "RII_WT_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_HEADER_DeletedBy",
                table: "RII_WT_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_HEADER_UpdatedBy",
                table: "RII_WT_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_IMPORT_LINE_CreatedBy",
                table: "RII_WT_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_IMPORT_LINE_DeletedBy",
                table: "RII_WT_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_IMPORT_LINE_HeaderId",
                table: "RII_WT_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_IMPORT_LINE_LineId",
                table: "RII_WT_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_IMPORT_LINE_UpdatedBy",
                table: "RII_WT_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_CreatedBy",
                table: "RII_WT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_DeletedBy",
                table: "RII_WT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_HeaderId",
                table: "RII_WT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_UpdatedBy",
                table: "RII_WT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_SERIAL_CreatedBy",
                table: "RII_WT_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_SERIAL_DeletedBy",
                table: "RII_WT_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_SERIAL_LineId",
                table: "RII_WT_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_LINE_SERIAL_UpdatedBy",
                table: "RII_WT_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_ROUTE_CreatedBy",
                table: "RII_WT_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_ROUTE_DeletedBy",
                table: "RII_WT_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_ROUTE_ImportLineId",
                table: "RII_WT_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_ROUTE_UpdatedBy",
                table: "RII_WT_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_TERMINAL_LINE_CreatedBy",
                table: "RII_WT_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_TERMINAL_LINE_DeletedBy",
                table: "RII_WT_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_TERMINAL_LINE_HeaderId",
                table: "RII_WT_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_TERMINAL_LINE_TerminalUserId",
                table: "RII_WT_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_TERMINAL_LINE_UpdatedBy",
                table: "RII_WT_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPageGroup_RII_PLATFORM_SIDEBARMENU_HEADER_MenuHeaderId",
                table: "PlatformPageGroup",
                column: "MenuHeaderId",
                principalTable: "RII_PLATFORM_SIDEBARMENU_HEADER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPageGroup_RII_PLATFORM_SIDEBARMENU_LINE_MenuLineId",
                table: "PlatformPageGroup",
                column: "MenuLineId",
                principalTable: "RII_PLATFORM_SIDEBARMENU_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPageGroup_RII_USERS_CreatedBy",
                table: "PlatformPageGroup",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPageGroup_RII_USERS_DeletedBy",
                table: "PlatformPageGroup",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPageGroup_RII_USERS_UpdatedBy",
                table: "PlatformPageGroup",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_HEADER_RII_USERS_CreatedBy",
                table: "RII_GR_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_HEADER_RII_USERS_DeletedBy",
                table: "RII_GR_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_HEADER_RII_USERS_UpdatedBy",
                table: "RII_GR_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_DOCUMENT_RII_USERS_CreatedBy",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_DOCUMENT_RII_USERS_DeletedBy",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_DOCUMENT_RII_USERS_UpdatedBy",
                table: "RII_GR_IMPORT_DOCUMENT",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_LINE_RII_GR_LINE_LineId",
                table: "RII_GR_IMPORT_LINE",
                column: "LineId",
                principalTable: "RII_GR_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_LINE_RII_USERS_CreatedBy",
                table: "RII_GR_IMPORT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_LINE_RII_USERS_DeletedBy",
                table: "RII_GR_IMPORT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_IMPORT_LINE_RII_USERS_UpdatedBy",
                table: "RII_GR_IMPORT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_RII_USERS_CreatedBy",
                table: "RII_GR_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_RII_USERS_DeletedBy",
                table: "RII_GR_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_RII_USERS_UpdatedBy",
                table: "RII_GR_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_USERS_CreatedBy",
                table: "RII_GR_LINE_SERIAL",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_USERS_DeletedBy",
                table: "RII_GR_LINE_SERIAL",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_USERS_UpdatedBy",
                table: "RII_GR_LINE_SERIAL",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_ROUTE_RII_USERS_CreatedBy",
                table: "RII_GR_ROUTE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_ROUTE_RII_USERS_DeletedBy",
                table: "RII_GR_ROUTE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_ROUTE_RII_USERS_UpdatedBy",
                table: "RII_GR_ROUTE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_HEADER_RII_USERS_CreatedBy",
                table: "RII_IC_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_HEADER_RII_USERS_DeletedBy",
                table: "RII_IC_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_HEADER_RII_USERS_UpdatedBy",
                table: "RII_IC_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_IMPORT_LINE_RII_USERS_CreatedBy",
                table: "RII_IC_IMPORT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_IMPORT_LINE_RII_USERS_DeletedBy",
                table: "RII_IC_IMPORT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_IMPORT_LINE_RII_USERS_UpdatedBy",
                table: "RII_IC_IMPORT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_ROUTE_RII_USERS_CreatedBy",
                table: "RII_IC_ROUTE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_ROUTE_RII_USERS_DeletedBy",
                table: "RII_IC_ROUTE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_ROUTE_RII_USERS_UpdatedBy",
                table: "RII_IC_ROUTE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_TERMINAL_LINE_RII_USERS_CreatedBy",
                table: "RII_IC_TERMINAL_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_TERMINAL_LINE_RII_USERS_DeletedBy",
                table: "RII_IC_TERMINAL_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_TERMINAL_LINE_RII_USERS_TerminalUserId",
                table: "RII_IC_TERMINAL_LINE",
                column: "TerminalUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_IC_TERMINAL_LINE_RII_USERS_UpdatedBy",
                table: "RII_IC_TERMINAL_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_PAGE_GROUP_RII_MOBILMENU_HEADER_MenuHeaderId",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "MenuHeaderId",
                principalTable: "RII_MOBILMENU_HEADER",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_PAGE_GROUP_RII_MOBILMENU_LINE_MenuLineId",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "MenuLineId",
                principalTable: "RII_MOBILMENU_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_PAGE_GROUP_RII_USERS_CreatedBy",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_PAGE_GROUP_RII_USERS_DeletedBy",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_PAGE_GROUP_RII_USERS_UpdatedBy",
                table: "RII_MOBIL_PAGE_GROUP",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_USER_GROUP_MATCH_RII_USERS_CreatedBy",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_USER_GROUP_MATCH_RII_USERS_DeletedBy",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_USER_GROUP_MATCH_RII_USERS_UpdatedBy",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBIL_USER_GROUP_MATCH_RII_USERS_UserId",
                table: "RII_MOBIL_USER_GROUP_MATCH",
                column: "UserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBILMENU_HEADER_RII_USERS_CreatedBy",
                table: "RII_MOBILMENU_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBILMENU_HEADER_RII_USERS_DeletedBy",
                table: "RII_MOBILMENU_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBILMENU_HEADER_RII_USERS_UpdatedBy",
                table: "RII_MOBILMENU_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBILMENU_LINE_RII_USERS_CreatedBy",
                table: "RII_MOBILMENU_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBILMENU_LINE_RII_USERS_DeletedBy",
                table: "RII_MOBILMENU_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_MOBILMENU_LINE_RII_USERS_UpdatedBy",
                table: "RII_MOBILMENU_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_SIDEBARMENU_HEADER_RII_USERS_CreatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_SIDEBARMENU_HEADER_RII_USERS_DeletedBy",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_SIDEBARMENU_HEADER_RII_USERS_UpdatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_SIDEBARMENU_LINE_RII_USERS_CreatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_SIDEBARMENU_LINE_RII_USERS_DeletedBy",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_SIDEBARMENU_LINE_RII_USERS_UpdatedBy",
                table: "RII_PLATFORM_SIDEBARMENU_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_USER_GROUP_MATCH_RII_USERS_CreatedBy",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_USER_GROUP_MATCH_RII_USERS_DeletedBy",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_USER_GROUP_MATCH_RII_USERS_UpdatedBy",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PLATFORM_USER_GROUP_MATCH_RII_USERS_UserId",
                table: "RII_PLATFORM_USER_GROUP_MATCH",
                column: "UserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_HEADER_RII_USERS_CreatedBy",
                table: "RII_PT_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_HEADER_RII_USERS_DeletedBy",
                table: "RII_PT_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_HEADER_RII_USERS_UpdatedBy",
                table: "RII_PT_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_IMPORT_LINE_RII_PT_LINE_LineId",
                table: "RII_PT_IMPORT_LINE",
                column: "LineId",
                principalTable: "RII_PT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_IMPORT_LINE_RII_USERS_CreatedBy",
                table: "RII_PT_IMPORT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_IMPORT_LINE_RII_USERS_DeletedBy",
                table: "RII_PT_IMPORT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_IMPORT_LINE_RII_USERS_UpdatedBy",
                table: "RII_PT_IMPORT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_LINE_RII_USERS_CreatedBy",
                table: "RII_PT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_LINE_RII_USERS_DeletedBy",
                table: "RII_PT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_LINE_RII_USERS_UpdatedBy",
                table: "RII_PT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_LINE_SERIAL_RII_USERS_CreatedBy",
                table: "RII_PT_LINE_SERIAL",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_LINE_SERIAL_RII_USERS_DeletedBy",
                table: "RII_PT_LINE_SERIAL",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_LINE_SERIAL_RII_USERS_UpdatedBy",
                table: "RII_PT_LINE_SERIAL",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_ROUTE_RII_USERS_CreatedBy",
                table: "RII_PT_ROUTE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_ROUTE_RII_USERS_DeletedBy",
                table: "RII_PT_ROUTE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_ROUTE_RII_USERS_UpdatedBy",
                table: "RII_PT_ROUTE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_TERMINAL_LINE_RII_USERS_CreatedBy",
                table: "RII_PT_TERMINAL_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_TERMINAL_LINE_RII_USERS_DeletedBy",
                table: "RII_PT_TERMINAL_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_TERMINAL_LINE_RII_USERS_TerminalUserId",
                table: "RII_PT_TERMINAL_LINE",
                column: "TerminalUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_PT_TERMINAL_LINE_RII_USERS_UpdatedBy",
                table: "RII_PT_TERMINAL_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_HEADER_RII_USERS_CreatedBy",
                table: "RII_SIT_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_HEADER_RII_USERS_DeletedBy",
                table: "RII_SIT_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_HEADER_RII_USERS_UpdatedBy",
                table: "RII_SIT_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_IMPORT_LINE_RII_SIT_LINE_LineId",
                table: "RII_SIT_IMPORT_LINE",
                column: "LineId",
                principalTable: "RII_SIT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_IMPORT_LINE_RII_USERS_CreatedBy",
                table: "RII_SIT_IMPORT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_IMPORT_LINE_RII_USERS_DeletedBy",
                table: "RII_SIT_IMPORT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_IMPORT_LINE_RII_USERS_UpdatedBy",
                table: "RII_SIT_IMPORT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_LINE_RII_USERS_CreatedBy",
                table: "RII_SIT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_LINE_RII_USERS_DeletedBy",
                table: "RII_SIT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_LINE_RII_USERS_UpdatedBy",
                table: "RII_SIT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_LINE_SERIAL_RII_USERS_CreatedBy",
                table: "RII_SIT_LINE_SERIAL",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_LINE_SERIAL_RII_USERS_DeletedBy",
                table: "RII_SIT_LINE_SERIAL",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_LINE_SERIAL_RII_USERS_UpdatedBy",
                table: "RII_SIT_LINE_SERIAL",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_ROUTE_RII_USERS_CreatedBy",
                table: "RII_SIT_ROUTE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_ROUTE_RII_USERS_DeletedBy",
                table: "RII_SIT_ROUTE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_ROUTE_RII_USERS_UpdatedBy",
                table: "RII_SIT_ROUTE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_TERMINAL_LINE_RII_USERS_CreatedBy",
                table: "RII_SIT_TERMINAL_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_TERMINAL_LINE_RII_USERS_DeletedBy",
                table: "RII_SIT_TERMINAL_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_TERMINAL_LINE_RII_USERS_TerminalUserId",
                table: "RII_SIT_TERMINAL_LINE",
                column: "TerminalUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SIT_TERMINAL_LINE_RII_USERS_UpdatedBy",
                table: "RII_SIT_TERMINAL_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_HEADER_RII_USERS_CreatedBy",
                table: "RII_SRT_HEADER",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_HEADER_RII_USERS_DeletedBy",
                table: "RII_SRT_HEADER",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_HEADER_RII_USERS_UpdatedBy",
                table: "RII_SRT_HEADER",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_IMPORT_LINE_RII_SRT_LINE_LineId",
                table: "RII_SRT_IMPORT_LINE",
                column: "LineId",
                principalTable: "RII_SRT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_IMPORT_LINE_RII_USERS_CreatedBy",
                table: "RII_SRT_IMPORT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_IMPORT_LINE_RII_USERS_DeletedBy",
                table: "RII_SRT_IMPORT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_IMPORT_LINE_RII_USERS_UpdatedBy",
                table: "RII_SRT_IMPORT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_LINE_RII_USERS_CreatedBy",
                table: "RII_SRT_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_LINE_RII_USERS_DeletedBy",
                table: "RII_SRT_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_LINE_RII_USERS_UpdatedBy",
                table: "RII_SRT_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_LINE_SERIAL_RII_USERS_CreatedBy",
                table: "RII_SRT_LINE_SERIAL",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_LINE_SERIAL_RII_USERS_DeletedBy",
                table: "RII_SRT_LINE_SERIAL",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_LINE_SERIAL_RII_USERS_UpdatedBy",
                table: "RII_SRT_LINE_SERIAL",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_ROUTE_RII_USERS_CreatedBy",
                table: "RII_SRT_ROUTE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_ROUTE_RII_USERS_DeletedBy",
                table: "RII_SRT_ROUTE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_ROUTE_RII_USERS_UpdatedBy",
                table: "RII_SRT_ROUTE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_TERMINAL_LINE_RII_USERS_CreatedBy",
                table: "RII_SRT_TERMINAL_LINE",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_TERMINAL_LINE_RII_USERS_DeletedBy",
                table: "RII_SRT_TERMINAL_LINE",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_TERMINAL_LINE_RII_USERS_TerminalUserId",
                table: "RII_SRT_TERMINAL_LINE",
                column: "TerminalUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_SRT_TERMINAL_LINE_RII_USERS_UpdatedBy",
                table: "RII_SRT_TERMINAL_LINE",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_CreatedBy",
                table: "RII_USER_AUTHORITY",
                column: "CreatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_DeletedBy",
                table: "RII_USER_AUTHORITY",
                column: "DeletedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_UpdatedBy",
                table: "RII_USER_AUTHORITY",
                column: "UpdatedBy",
                principalTable: "RII_USERS",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_CreatedBy",
                table: "RII_USER_AUTHORITY");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_DeletedBy",
                table: "RII_USER_AUTHORITY");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_USER_AUTHORITY_RII_USERS_UpdatedBy",
                table: "RII_USER_AUTHORITY");

            migrationBuilder.DropTable(
                name: "RII_GR_IMPORT_DOCUMENT");

            migrationBuilder.DropTable(
                name: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_GR_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_IC_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_IC_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_MOBIL_USER_PAGE_GROUP_MATCH");

            migrationBuilder.DropTable(
                name: "RII_PLATFORM_USER_GROUP_MATCH");

            migrationBuilder.DropTable(
                name: "RII_PT_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_PT_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_PT_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_SIT_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_SIT_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_SIT_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_SRT_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_SRT_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_SRT_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_USER_SESSION");

            migrationBuilder.DropTable(
                name: "RII_WI_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_WI_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_WI_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_WO_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_WO_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_WO_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_WT_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_WT_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_WT_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_GR_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_IC_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_MOBIL_PAGE_GROUP");

            migrationBuilder.DropTable(
                name: "RII_MOBIL_USER_GROUP_MATCH");

            migrationBuilder.DropTable(
                name: "PlatformPageGroup");

            migrationBuilder.DropTable(
                name: "RII_PT_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_SIT_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_SRT_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_WI_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_WO_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_WT_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_GR_LINE");

            migrationBuilder.DropTable(
                name: "RII_IC_HEADER");

            migrationBuilder.DropTable(
                name: "RII_MOBILMENU_LINE");

            migrationBuilder.DropTable(
                name: "RII_PLATFORM_SIDEBARMENU_LINE");

            migrationBuilder.DropTable(
                name: "RII_PT_LINE");

            migrationBuilder.DropTable(
                name: "RII_SIT_LINE");

            migrationBuilder.DropTable(
                name: "RII_SRT_LINE");

            migrationBuilder.DropTable(
                name: "RII_WI_LINE");

            migrationBuilder.DropTable(
                name: "RII_WO_LINE");

            migrationBuilder.DropTable(
                name: "RII_WT_LINE");

            migrationBuilder.DropTable(
                name: "RII_GR_HEADER");

            migrationBuilder.DropTable(
                name: "RII_MOBILMENU_HEADER");

            migrationBuilder.DropTable(
                name: "RII_PLATFORM_SIDEBARMENU_HEADER");

            migrationBuilder.DropTable(
                name: "RII_PT_HEADER");

            migrationBuilder.DropTable(
                name: "RII_SIT_HEADER");

            migrationBuilder.DropTable(
                name: "RII_SRT_HEADER");

            migrationBuilder.DropTable(
                name: "RII_WI_HEADER");

            migrationBuilder.DropTable(
                name: "RII_WO_HEADER");

            migrationBuilder.DropTable(
                name: "RII_WT_HEADER");

            migrationBuilder.DropTable(
                name: "RII_USERS");

            migrationBuilder.DropTable(
                name: "RII_USER_AUTHORITY");
        }
    }
}
