using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceAllocationOrchestration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_SA_ALLOCATION_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErpOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    ProcessType = table.Column<int>(type: "int", nullable: false),
                    RequestedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AllocatedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    FulfilledQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PriorityNo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SourceModule = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SourceHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    SourceLineId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_RII_SA_ALLOCATION_LINE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_SA_SERVICE_CASE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    IncomingStockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    IncomingStockId = table.Column<long>(type: "bigint", nullable: true),
                    IncomingSerialNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IntakeWarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentWarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    DiagnosisNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_RII_SA_SERVICE_CASE", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RII_SA_DOCUMENT_LINK",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessEntityType = table.Column<int>(type: "int", nullable: false),
                    BusinessEntityId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceCaseId = table.Column<long>(type: "bigint", nullable: true),
                    OrderAllocationLineId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentModule = table.Column<int>(type: "int", nullable: false),
                    DocumentHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentLineId = table.Column<long>(type: "bigint", nullable: true),
                    LinkPurpose = table.Column<int>(type: "int", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: false),
                    FromWarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    ToWarehouseId = table.Column<long>(type: "bigint", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_RII_SA_DOCUMENT_LINK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SA_DOCUMENT_LINK_RII_SA_ALLOCATION_LINE_OrderAllocationLineId",
                        column: x => x.OrderAllocationLineId,
                        principalTable: "RII_SA_ALLOCATION_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SA_DOCUMENT_LINK_RII_SA_SERVICE_CASE_ServiceCaseId",
                        column: x => x.ServiceCaseId,
                        principalTable: "RII_SA_SERVICE_CASE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RII_SA_SERVICE_CASE_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCaseId = table.Column<long>(type: "bigint", nullable: false),
                    LineType = table.Column<int>(type: "int", nullable: false),
                    ProcessType = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_RII_SA_SERVICE_CASE_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SA_SERVICE_CASE_LINE_RII_SA_SERVICE_CASE_ServiceCaseId",
                        column: x => x.ServiceCaseId,
                        principalTable: "RII_SA_SERVICE_CASE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllocationLine_ErpOrderId",
                table: "RII_SA_ALLOCATION_LINE",
                column: "ErpOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationLine_IsDeleted",
                table: "RII_SA_ALLOCATION_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationLine_Queue",
                table: "RII_SA_ALLOCATION_LINE",
                columns: new[] { "StockCode", "Status", "PriorityNo" });

            migrationBuilder.CreateIndex(
                name: "IX_AllocationLine_SourceLineId",
                table: "RII_SA_ALLOCATION_LINE",
                column: "SourceLineId");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationLine_StockCode",
                table: "RII_SA_ALLOCATION_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationLine_StockId",
                table: "RII_SA_ALLOCATION_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLink_BusinessEntity",
                table: "RII_SA_DOCUMENT_LINK",
                columns: new[] { "BusinessEntityType", "BusinessEntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLink_DocumentHeader",
                table: "RII_SA_DOCUMENT_LINK",
                columns: new[] { "DocumentModule", "DocumentHeaderId" });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLink_IsDeleted",
                table: "RII_SA_DOCUMENT_LINK",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLink_OrderAllocationLineId",
                table: "RII_SA_DOCUMENT_LINK",
                column: "OrderAllocationLineId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentLink_ServiceCaseId",
                table: "RII_SA_DOCUMENT_LINK",
                column: "ServiceCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCase_CurrentWarehouseId",
                table: "RII_SA_SERVICE_CASE",
                column: "CurrentWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCase_CustomerCode",
                table: "RII_SA_SERVICE_CASE",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCase_IsDeleted",
                table: "RII_SA_SERVICE_CASE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCase_Status",
                table: "RII_SA_SERVICE_CASE",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UX_ServiceCase_CaseNo",
                table: "RII_SA_SERVICE_CASE",
                column: "CaseNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCaseLine_ErpOrderId",
                table: "RII_SA_SERVICE_CASE_LINE",
                column: "ErpOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCaseLine_IsDeleted",
                table: "RII_SA_SERVICE_CASE_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCaseLine_ServiceCaseId",
                table: "RII_SA_SERVICE_CASE_LINE",
                column: "ServiceCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCaseLine_StockCode",
                table: "RII_SA_SERVICE_CASE_LINE",
                column: "StockCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_SA_DOCUMENT_LINK");

            migrationBuilder.DropTable(
                name: "RII_SA_SERVICE_CASE_LINE");

            migrationBuilder.DropTable(
                name: "RII_SA_ALLOCATION_LINE");

            migrationBuilder.DropTable(
                name: "RII_SA_SERVICE_CASE");
        }
    }
}
