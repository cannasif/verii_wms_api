using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLineOrderQuantityAndLineSerialWarehouseRefs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_WT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_WT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_WT_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_WO_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_WO_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_WO_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_WI_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_WI_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_WI_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_SRT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_SRT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_SRT_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_SIT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_SIT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_SIT_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_SH_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_SH_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_SH_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_PT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_PT_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_PT_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_PR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_PR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_PR_LINE",
                type: "decimal(18,6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceWarehouseId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TargetWarehouseId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SiparisMiktar",
                table: "RII_GR_LINE",
                type: "decimal(18,6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_WT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_WT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_WT_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_WO_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_WO_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_WO_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_WI_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_WI_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_WI_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_SRT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_SRT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_SRT_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_SIT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_SIT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_SIT_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_SH_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_SH_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_SH_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_PT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_PT_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_PT_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_PR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_PR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_PR_LINE");

            migrationBuilder.DropColumn(
                name: "SourceWarehouseId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "TargetWarehouseId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "SiparisMiktar",
                table: "RII_GR_LINE");
        }
    }
}
