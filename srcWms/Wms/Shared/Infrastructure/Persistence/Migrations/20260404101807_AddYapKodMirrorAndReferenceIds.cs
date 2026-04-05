using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wms.Shared.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddYapKodMirrorAndReferenceIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_WT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WT_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_WT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WO_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_WO_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WO_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_WO_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WI_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_WI_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WI_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_WI_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SRT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_SRT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SRT_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_SRT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SIT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_SIT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SIT_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_SIT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SH_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_SH_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SH_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_SH_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_PT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PT_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_PT_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PR_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_PR_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PR_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_PR_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_PR_HEADER",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_P_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_IC_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(35)",
                oldMaxLength: 35,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_IC_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_GR_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_GR_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_GR_IMPORT_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "YapKodId",
                table: "RII_GR_IMPORT_LINE",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RII_WMS_YAPKOD",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YapKodCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    YapAcik = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    YplndrStokKod = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
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
                    table.PrimaryKey("PK_RII_WMS_YAPKOD", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_YapKodId",
                table: "RII_PR_HEADER",
                column: "YapKodId");

            migrationBuilder.CreateIndex(
                name: "IX_PLine_YapKodId",
                table: "RII_P_LINE",
                column: "YapKodId");

            migrationBuilder.CreateIndex(
                name: "IX_YapKod_IsDeleted",
                table: "RII_WMS_YAPKOD",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_YapKod_YapAcik",
                table: "RII_WMS_YAPKOD",
                column: "YapAcik");

            migrationBuilder.CreateIndex(
                name: "IX_YapKod_YapKodCode",
                table: "RII_WMS_YAPKOD",
                column: "YapKodCode",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_WMS_YAPKOD");

            migrationBuilder.DropIndex(
                name: "IX_PrHeader_YapKodId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropIndex(
                name: "IX_PLine_YapKodId",
                table: "RII_P_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_WT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_WT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_WO_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_WO_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_WI_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_WI_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_SRT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_SRT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_SIT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_SIT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_SH_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_SH_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_PT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_PT_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_PR_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_PR_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_PR_HEADER");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_P_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_IC_IMPORT_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_GR_LINE");

            migrationBuilder.DropColumn(
                name: "YapKodId",
                table: "RII_GR_IMPORT_LINE");

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WT_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WO_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WO_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WI_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_WI_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SRT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SRT_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SIT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SIT_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SH_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_SH_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PT_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PR_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_PR_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_IC_IMPORT_LINE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_GR_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_GR_IMPORT_LINE",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
