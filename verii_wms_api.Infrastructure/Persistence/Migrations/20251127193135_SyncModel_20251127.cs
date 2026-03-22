using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20251127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_ImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_WT_ROUTE");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_WO_ROUTE");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_WI_ROUTE");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_SRT_ROUTE");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_SIT_ROUTE");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_PT_ROUTE");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RII_GR_ROUTE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_WT_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_WT_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_WO_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_WO_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_WO_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_WO_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_WI_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_WI_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_WI_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_WI_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_SRT_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_SRT_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_SRT_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_SRT_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_SIT_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_SIT_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_SIT_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_SIT_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_PT_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_PT_HEADER",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_PT_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_PT_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_IC_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_IC_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ImportLineId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "ClientKey",
                table: "RII_GR_LINE_SERIAL",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GrImportLineId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_GR_HEADER",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_GR_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "0",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "RII_GR_HEADER",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RII_NOTIFICATION",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Channel = table.Column<byte>(type: "tinyint", nullable: false),
                    Severity = table.Column<byte>(type: "tinyint", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecipientUserId = table.Column<long>(type: "bigint", nullable: true),
                    RecipientTerminalUserId = table.Column<long>(type: "bigint", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RelatedEntityId = table.Column<long>(type: "bigint", nullable: true),
                    ActionUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TerminalActionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_RII_NOTIFICATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_NOTIFICATION_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_NOTIFICATION_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_NOTIFICATION_RII_USERS_RecipientTerminalUserId",
                        column: x => x.RecipientTerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_NOTIFICATION_RII_USERS_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_NOTIFICATION_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrImportLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Channel",
                table: "RII_NOTIFICATION",
                column: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_DeliveredAt",
                table: "RII_NOTIFICATION",
                column: "DeliveredAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_IsDeleted",
                table: "RII_NOTIFICATION",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_IsRead",
                table: "RII_NOTIFICATION",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_RII_NOTIFICATION_CreatedBy",
                table: "RII_NOTIFICATION",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_NOTIFICATION_DeletedBy",
                table: "RII_NOTIFICATION",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_NOTIFICATION_RecipientTerminalUserId",
                table: "RII_NOTIFICATION",
                column: "RecipientTerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_NOTIFICATION_RecipientUserId",
                table: "RII_NOTIFICATION",
                column: "RecipientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_NOTIFICATION_UpdatedBy",
                table: "RII_NOTIFICATION",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_GrLineSerial_GrImportLine",
                table: "RII_GR_LINE_SERIAL",
                column: "ImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_GrImportLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrLineSerial_GrImportLine",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_GrImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_NOTIFICATION");

            migrationBuilder.DropIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_WT_HEADER");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_WO_HEADER");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_WI_HEADER");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_SRT_HEADER");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_SIT_HEADER");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_PT_HEADER");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_IC_HEADER");

            migrationBuilder.DropColumn(
                name: "ClientKey",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "GrImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RII_GR_HEADER");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_WT_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_WT_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_WO_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_WO_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_WO_HEADER",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_WO_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "0");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_WI_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_WI_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_WI_HEADER",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_WI_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "0");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_SRT_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_SRT_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_SRT_HEADER",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_SRT_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "0");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_SIT_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_SIT_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_SIT_HEADER",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_SIT_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "0");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_PT_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_PT_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPlanned",
                table: "RII_PT_HEADER",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_PT_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "0");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_IC_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RII_GR_ROUTE",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ImportLineId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PlannedDate",
                table: "RII_GR_HEADER",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "RII_GR_HEADER",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "0");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_ImportLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "ImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
