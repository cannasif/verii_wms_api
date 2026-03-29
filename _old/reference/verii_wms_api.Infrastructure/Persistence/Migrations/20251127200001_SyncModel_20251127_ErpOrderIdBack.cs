using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20251127_ErpOrderIdBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_WT_LINE",
                newName: "ErpOrderId");

            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_WO_LINE",
                newName: "ErpOrderId");

            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_WI_LINE",
                newName: "ErpOrderId");

            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_SRT_LINE",
                newName: "ErpOrderId");

            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_SIT_LINE",
                newName: "ErpOrderId");

            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_PT_LINE",
                newName: "ErpOrderId");

            migrationBuilder.RenameColumn(
                name: "ErpOrderLineNo",
                table: "RII_GR_LINE",
                newName: "ErpOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_WT_LINE",
                newName: "ErpOrderLineNo");

            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_WO_LINE",
                newName: "ErpOrderLineNo");

            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_WI_LINE",
                newName: "ErpOrderLineNo");

            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_SRT_LINE",
                newName: "ErpOrderLineNo");

            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_SIT_LINE",
                newName: "ErpOrderLineNo");

            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_PT_LINE",
                newName: "ErpOrderLineNo");

            migrationBuilder.RenameColumn(
                name: "ErpOrderId",
                table: "RII_GR_LINE",
                newName: "ErpOrderLineNo");
        }
    }
}
