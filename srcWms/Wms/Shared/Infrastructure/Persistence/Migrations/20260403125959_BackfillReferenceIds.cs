using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BackfillReferenceIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_WT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_WT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_WT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_WT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_WT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_WO_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_WO_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_WO_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_WO_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_WO_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_WI_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_WI_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_WI_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_WI_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_WI_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_SRT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_SRT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_SRT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_SRT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_SRT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_SIT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_SIT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_SIT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_SIT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_SIT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_SH_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_SH_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_SH_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_SH_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_SH_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_PT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_PT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_PT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_PT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_PT_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_PR_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_PR_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_PR_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_PR_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_PR_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_PR_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_P_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_P_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "RII_P_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_IC_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "RII_IC_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_GR_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StockId",
                table: "RII_GR_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "RII_GR_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WtLine_StockId",
                table: "RII_WT_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_TrImportLine_StockId",
                table: "RII_WT_IMPORT_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_TrHeader_CustomerId",
                table: "RII_WT_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrHeader_SourceWarehouseId",
                table: "RII_WT_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrHeader_TargetWarehouseId",
                table: "RII_WT_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WoLine_StockId",
                table: "RII_WO_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_WoImportLine_StockId",
                table: "RII_WO_IMPORT_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_CustomerId",
                table: "RII_WO_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_SourceWarehouseId",
                table: "RII_WO_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WoHeader_TargetWarehouseId",
                table: "RII_WO_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WiLine_StockId",
                table: "RII_WI_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_WiImportLine_StockId",
                table: "RII_WI_IMPORT_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_CustomerId",
                table: "RII_WI_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_SourceWarehouseId",
                table: "RII_WI_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WiHeader_TargetWarehouseId",
                table: "RII_WI_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtHeader_CustomerId",
                table: "RII_SRT_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtHeader_SourceWarehouseId",
                table: "RII_SRT_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SrtHeader_TargetWarehouseId",
                table: "RII_SRT_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_CustomerId",
                table: "RII_SIT_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_SourceWarehouseId",
                table: "RII_SIT_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SitHeader_TargetWarehouseId",
                table: "RII_SIT_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ShLine_StockId",
                table: "RII_SH_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_ShImportLine_StockId",
                table: "RII_SH_IMPORT_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_CustomerId",
                table: "RII_SH_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_SourceWarehouseId",
                table: "RII_SH_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_TargetWarehouseId",
                table: "RII_SH_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_CustomerId",
                table: "RII_PT_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_SourceWarehouseId",
                table: "RII_PT_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PtHeader_TargetWarehouseId",
                table: "RII_PT_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_CustomerId",
                table: "RII_PR_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_SourceWarehouseId",
                table: "RII_PR_HEADER",
                column: "SourceWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_StockId",
                table: "RII_PR_HEADER",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_TargetWarehouseId",
                table: "RII_PR_HEADER",
                column: "TargetWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PLine_StockId",
                table: "RII_P_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_CustomerId",
                table: "RII_P_HEADER",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_WarehouseId",
                table: "RII_P_HEADER",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_IcHeader_WarehouseId",
                table: "RII_IC_HEADER",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_GrLine_StockId",
                table: "RII_GR_LINE",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_GrHeader_CustomerId",
                table: "RII_GR_HEADER",
                column: "CustomerId");

            migrationBuilder.Sql("""
UPDATE H
SET H.CustomerId = C.Id
FROM RII_GR_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_WI_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_WO_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_WT_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_SH_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_SIT_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_SRT_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_PR_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_PT_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.CustomerId = C.Id
FROM RII_P_HEADER H
INNER JOIN RII_WMS_CUSTOMER C ON C.CustomerCode = H.CustomerCode AND C.IsDeleted = 0
WHERE H.CustomerId IS NULL AND H.CustomerCode IS NOT NULL;

UPDATE H
SET H.WarehouseId = W.Id
FROM RII_IC_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.WarehouseCode AND W.IsDeleted = 0
WHERE H.WarehouseId IS NULL AND H.WarehouseCode IS NOT NULL;

UPDATE H
SET H.WarehouseId = W.Id
FROM RII_P_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.WarehouseCode AND W.IsDeleted = 0
WHERE H.WarehouseId IS NULL AND H.WarehouseCode IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_WI_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_WI_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_WO_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_WO_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_WT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_WT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_SH_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_SH_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_SIT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_SIT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_SRT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_SRT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_PR_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_PR_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE H
SET H.SourceWarehouseId = W.Id
FROM RII_PT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.SourceWarehouse AND W.IsDeleted = 0
WHERE H.SourceWarehouseId IS NULL AND H.SourceWarehouse IS NOT NULL;

UPDATE H
SET H.TargetWarehouseId = W.Id
FROM RII_PT_HEADER H
INNER JOIN RII_WMS_WAREHOUSE W ON CAST(W.WarehouseCode AS nvarchar(20)) = H.TargetWarehouse AND W.IsDeleted = 0
WHERE H.TargetWarehouseId IS NULL AND H.TargetWarehouse IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_GR_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_GR_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_WI_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_WI_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_WO_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_WO_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_WT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_WT_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_SH_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_SH_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_SIT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_SIT_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_SRT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_SRT_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_PR_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_PR_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE H
SET H.StockId = S.Id
FROM RII_PR_HEADER H
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = H.StockCode AND S.IsDeleted = 0
WHERE H.StockId IS NULL AND H.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_PT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_PT_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_IC_IMPORT_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;

UPDATE L
SET L.StockId = S.Id
FROM RII_P_LINE L
INNER JOIN RII_WMS_STOCK S ON S.ErpStockCode = L.StockCode AND S.IsDeleted = 0
WHERE L.StockId IS NULL AND L.StockCode IS NOT NULL;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WtLine_StockId",
                table: "RII_WT_LINE");

            migrationBuilder.DropIndex(
                name: "IX_TrImportLine_StockId",
                table: "RII_WT_IMPORT_LINE");

            migrationBuilder.DropIndex(
                name: "IX_TrHeader_CustomerId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_TrHeader_SourceWarehouseId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_TrHeader_TargetWarehouseId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_WoLine_StockId",
                table: "RII_WO_LINE");

            migrationBuilder.DropIndex(
                name: "IX_WoImportLine_StockId",
                table: "RII_WO_IMPORT_LINE");

            migrationBuilder.DropIndex(
                name: "IX_WoHeader_CustomerId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_WoHeader_SourceWarehouseId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_WoHeader_TargetWarehouseId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_WiLine_StockId",
                table: "RII_WI_LINE");

            migrationBuilder.DropIndex(
                name: "IX_WiImportLine_StockId",
                table: "RII_WI_IMPORT_LINE");

            migrationBuilder.DropIndex(
                name: "IX_WiHeader_CustomerId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_WiHeader_SourceWarehouseId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_WiHeader_TargetWarehouseId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_SrtHeader_CustomerId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_SrtHeader_SourceWarehouseId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_SrtHeader_TargetWarehouseId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_SitHeader_CustomerId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_SitHeader_SourceWarehouseId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_SitHeader_TargetWarehouseId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_ShLine_StockId",
                table: "RII_SH_LINE");

            migrationBuilder.DropIndex(
                name: "IX_ShImportLine_StockId",
                table: "RII_SH_IMPORT_LINE");

            migrationBuilder.DropIndex(
                name: "IX_ShHeader_CustomerId",
                table: "RII_SH_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_ShHeader_SourceWarehouseId",
                table: "RII_SH_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_ShHeader_TargetWarehouseId",
                table: "RII_SH_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PtHeader_CustomerId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PtHeader_SourceWarehouseId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PtHeader_TargetWarehouseId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PrHeader_CustomerId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PrHeader_SourceWarehouseId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PrHeader_StockId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PrHeader_TargetWarehouseId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PLine_StockId",
                table: "RII_P_LINE");

            migrationBuilder.DropIndex(
                name: "IX_PHeader_CustomerId",
                table: "RII_P_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PHeader_WarehouseId",
                table: "RII_P_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_IcHeader_WarehouseId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_GrLine_StockId",
                table: "RII_GR_LINE");

            migrationBuilder.DropIndex(
                name: "IX_GrHeader_CustomerId",
                table: "RII_GR_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_WT_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_WT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_WO_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_WO_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_WI_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_WI_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_SRT_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_SRT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_SIT_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_SIT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_SH_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_SH_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_SH_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_SH_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_SH_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_PT_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_PT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_PR_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_PR_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_P_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_P_HEADER");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "RII_P_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_IC_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_GR_LINE");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "RII_GR_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "RII_GR_HEADER");
        }
    }
}
