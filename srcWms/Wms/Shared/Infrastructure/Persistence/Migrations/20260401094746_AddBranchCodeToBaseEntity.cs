using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Wms.Infrastructure.Persistence.Context;

#nullable disable

namespace Wms.Infrastructure.Persistence.Migrations;

[DbContext(typeof(WmsDbContext))]
[Migration("20260401094746_AddBranchCodeToBaseEntity")]
public partial class AddBranchCodeToBaseEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var table in AllBranchScopedTables)
        {
            AddBranchCodeColumnIfMissing(migrationBuilder, table);
        }

        foreach (var pair in HeaderBackfillTables)
        {
            BackfillFromHeader(migrationBuilder, pair.HeaderTable, pair.ChildTable, pair.HeaderIdColumn);
        }

        foreach (var pair in RouteBackfillTables)
        {
            BackfillRouteFromImportLine(migrationBuilder, pair.HeaderTable, pair.ImportLineTable, pair.RouteTable);
        }

        foreach (var pair in LineSerialBackfillTables)
        {
            BackfillLineSerialFromLine(migrationBuilder, pair.HeaderTable, pair.LineTable, pair.LineSerialTable, pair.LineIdColumn);
        }

        BackfillFromHeader(migrationBuilder, "RII_P_HEADER", "RII_P_PACKAGE", "PackingHeaderId");
        BackfillPLineFromHeader(migrationBuilder);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        foreach (var table in AllBranchScopedTables.Reverse())
        {
            DropBranchCodeColumnIfExists(migrationBuilder, table);
        }
    }

    private static void AddBranchCodeColumnIfMissing(MigrationBuilder migrationBuilder, string table)
    {
        migrationBuilder.Sql($"""
IF OBJECT_ID(N'[{table}]', N'U') IS NOT NULL
   AND COL_LENGTH('{table}', 'BranchCode') IS NULL
BEGIN
    ALTER TABLE [{table}]
    ADD [BranchCode] nvarchar(10) NOT NULL
        CONSTRAINT [DF_{table}_BranchCode] DEFAULT N'0';
END
""");
    }

    private static void DropBranchCodeColumnIfExists(MigrationBuilder migrationBuilder, string table)
    {
        migrationBuilder.Sql($"""
IF OBJECT_ID(N'[{table}]', N'U') IS NOT NULL
   AND COL_LENGTH('{table}', 'BranchCode') IS NOT NULL
BEGIN
    IF EXISTS (
        SELECT 1
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c
            ON c.default_object_id = dc.object_id
        INNER JOIN sys.tables t
            ON t.object_id = c.object_id
        WHERE t.name = '{table}' AND c.name = 'BranchCode'
    )
    BEGIN
        DECLARE @sql nvarchar(max);
        SELECT @sql = N'ALTER TABLE [{table}] DROP CONSTRAINT [' + dc.name + N']'
        FROM sys.default_constraints dc
        INNER JOIN sys.columns c
            ON c.default_object_id = dc.object_id
        INNER JOIN sys.tables t
            ON t.object_id = c.object_id
        WHERE t.name = '{table}' AND c.name = 'BranchCode';

        EXEC sp_executesql @sql;
    END

    ALTER TABLE [{table}] DROP COLUMN [BranchCode];
END
""");
    }

    private static void BackfillFromHeader(MigrationBuilder migrationBuilder, string headerTable, string childTable, string headerIdColumn)
    {
        migrationBuilder.Sql($"""
IF OBJECT_ID(N'[{headerTable}]', N'U') IS NOT NULL
   AND OBJECT_ID(N'[{childTable}]', N'U') IS NOT NULL
   AND COL_LENGTH('{childTable}', 'BranchCode') IS NOT NULL
   AND COL_LENGTH('{childTable}', '{headerIdColumn}') IS NOT NULL
BEGIN
    UPDATE child
    SET child.BranchCode = header.BranchCode
    FROM [{childTable}] child
    INNER JOIN [{headerTable}] header ON header.Id = child.[{headerIdColumn}]
    WHERE child.BranchCode IS NULL OR child.BranchCode = N'0';
END
""");
    }

    private static void BackfillRouteFromImportLine(MigrationBuilder migrationBuilder, string headerTable, string importLineTable, string routeTable)
    {
        migrationBuilder.Sql($"""
IF OBJECT_ID(N'[{headerTable}]', N'U') IS NOT NULL
   AND OBJECT_ID(N'[{importLineTable}]', N'U') IS NOT NULL
   AND OBJECT_ID(N'[{routeTable}]', N'U') IS NOT NULL
   AND COL_LENGTH('{routeTable}', 'BranchCode') IS NOT NULL
BEGIN
    UPDATE route
    SET route.BranchCode = header.BranchCode
    FROM [{routeTable}] route
    INNER JOIN [{importLineTable}] importLine ON importLine.Id = route.ImportLineId
    INNER JOIN [{headerTable}] header ON header.Id = importLine.HeaderId
    WHERE route.BranchCode IS NULL OR route.BranchCode = N'0';
END
""");
    }

    private static void BackfillLineSerialFromLine(MigrationBuilder migrationBuilder, string headerTable, string lineTable, string lineSerialTable, string lineIdColumn)
    {
        migrationBuilder.Sql($"""
IF OBJECT_ID(N'[{headerTable}]', N'U') IS NOT NULL
   AND OBJECT_ID(N'[{lineTable}]', N'U') IS NOT NULL
   AND OBJECT_ID(N'[{lineSerialTable}]', N'U') IS NOT NULL
   AND COL_LENGTH('{lineSerialTable}', 'BranchCode') IS NOT NULL
   AND COL_LENGTH('{lineSerialTable}', '{lineIdColumn}') IS NOT NULL
BEGIN
    UPDATE serial
    SET serial.BranchCode = header.BranchCode
    FROM [{lineSerialTable}] serial
    INNER JOIN [{lineTable}] line ON line.Id = serial.[{lineIdColumn}]
    INNER JOIN [{headerTable}] header ON header.Id = line.HeaderId
    WHERE serial.BranchCode IS NULL OR serial.BranchCode = N'0';
END
""");
    }

    private static void BackfillPLineFromHeader(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
IF OBJECT_ID(N'[RII_P_HEADER]', N'U') IS NOT NULL
   AND OBJECT_ID(N'[RII_P_LINE]', N'U') IS NOT NULL
   AND COL_LENGTH('RII_P_LINE', 'BranchCode') IS NOT NULL
BEGIN
    UPDATE line
    SET line.BranchCode = header.BranchCode
    FROM [RII_P_LINE] line
    INNER JOIN [RII_P_HEADER] header ON header.Id = line.PackingHeaderId
    WHERE line.BranchCode IS NULL OR line.BranchCode = N'0';
END
""");
    }

    private static readonly string[] AllBranchScopedTables =
    {
        "RII_PERMISSION_DEFINITIONS",
        "RII_PERMISSION_GROUPS",
        "RII_PERMISSION_GROUP_PERMISSIONS",
        "RII_USER_PERMISSION_GROUPS",
        "RII_JOB_FAILURE_LOG",
        "RII_NOTIFICATION",
        "RII_SMTP_SETTING",
        "RII_GR_PARAMETER",
        "RII_IC_PARAMETER",
        "RII_P_PARAMETER",
        "RII_PR_PARAMETER",
        "RII_PT_PARAMETER",
        "RII_SH_PARAMETER",
        "RII_SIT_PARAMETER",
        "RII_SRT_PARAMETER",
        "RII_WI_PARAMETER",
        "RII_WO_PARAMETER",
        "RII_WT_PARAMETER",
        "RII_GR_HEADER",
        "RII_GR_LINE",
        "RII_GR_IMPORT_LINE",
        "RII_GR_ROUTE",
        "RII_GR_LINE_SERIAL",
        "RII_GR_TERMINAL_LINE",
        "RII_GR_IMPORT_DOCUMENT",
        "RII_IC_HEADER",
        "RII_IC_IMPORT_LINE",
        "RII_IC_ROUTE",
        "RII_IC_TERMINAL_LINE",
        "RII_P_HEADER",
        "RII_P_LINE",
        "RII_P_PACKAGE",
        "RII_PR_HEADER",
        "RII_PR_LINE",
        "RII_PR_IMPORT_LINE",
        "RII_PR_ROUTE",
        "RII_PR_LINE_SERIAL",
        "RII_PR_TERMINAL_LINE",
        "RII_PR_HEADER_SERIAL",
        "RII_PT_HEADER",
        "RII_PT_LINE",
        "RII_PT_IMPORT_LINE",
        "RII_PT_ROUTE",
        "RII_PT_LINE_SERIAL",
        "RII_PT_TERMINAL_LINE",
        "RII_SH_HEADER",
        "RII_SH_LINE",
        "RII_SH_IMPORT_LINE",
        "RII_SH_ROUTE",
        "RII_SH_LINE_SERIAL",
        "RII_SH_TERMINAL_LINE",
        "RII_SIT_HEADER",
        "RII_SIT_LINE",
        "RII_SIT_IMPORT_LINE",
        "RII_SIT_ROUTE",
        "RII_SIT_LINE_SERIAL",
        "RII_SIT_TERMINAL_LINE",
        "RII_SRT_HEADER",
        "RII_SRT_LINE",
        "RII_SRT_IMPORT_LINE",
        "RII_SRT_ROUTE",
        "RII_SRT_LINE_SERIAL",
        "RII_SRT_TERMINAL_LINE",
        "RII_WI_HEADER",
        "RII_WI_LINE",
        "RII_WI_IMPORT_LINE",
        "RII_WI_ROUTE",
        "RII_WI_LINE_SERIAL",
        "RII_WI_TERMINAL_LINE",
        "RII_WO_HEADER",
        "RII_WO_LINE",
        "RII_WO_IMPORT_LINE",
        "RII_WO_ROUTE",
        "RII_WO_LINE_SERIAL",
        "RII_WO_TERMINAL_LINE",
        "RII_WT_HEADER",
        "RII_WT_LINE",
        "RII_WT_IMPORT_LINE",
        "RII_WT_ROUTE",
        "RII_WT_LINE_SERIAL",
        "RII_WT_TERMINAL_LINE",
        "RII_PASSWORD_RESET_REQUEST",
        "RII_USERS",
        "RII_USER_AUTHORITY",
        "RII_USER_DETAIL",
        "RII_USER_SESSION"
    };

    private static readonly (string HeaderTable, string ChildTable, string HeaderIdColumn)[] HeaderBackfillTables =
    {
        ("RII_GR_HEADER", "RII_GR_LINE", "HeaderId"),
        ("RII_GR_HEADER", "RII_GR_IMPORT_LINE", "HeaderId"),
        ("RII_GR_HEADER", "RII_GR_TERMINAL_LINE", "HeaderId"),
        ("RII_GR_HEADER", "RII_GR_IMPORT_DOCUMENT", "HeaderId"),
        ("RII_IC_HEADER", "RII_IC_IMPORT_LINE", "HeaderId"),
        ("RII_IC_HEADER", "RII_IC_TERMINAL_LINE", "HeaderId"),
        ("RII_PR_HEADER", "RII_PR_LINE", "HeaderId"),
        ("RII_PR_HEADER", "RII_PR_IMPORT_LINE", "HeaderId"),
        ("RII_PR_HEADER", "RII_PR_TERMINAL_LINE", "HeaderId"),
        ("RII_PR_HEADER", "RII_PR_HEADER_SERIAL", "HeaderId"),
        ("RII_PT_HEADER", "RII_PT_LINE", "HeaderId"),
        ("RII_PT_HEADER", "RII_PT_IMPORT_LINE", "HeaderId"),
        ("RII_PT_HEADER", "RII_PT_TERMINAL_LINE", "HeaderId"),
        ("RII_SH_HEADER", "RII_SH_LINE", "HeaderId"),
        ("RII_SH_HEADER", "RII_SH_IMPORT_LINE", "HeaderId"),
        ("RII_SH_HEADER", "RII_SH_TERMINAL_LINE", "HeaderId"),
        ("RII_SIT_HEADER", "RII_SIT_LINE", "HeaderId"),
        ("RII_SIT_HEADER", "RII_SIT_IMPORT_LINE", "HeaderId"),
        ("RII_SIT_HEADER", "RII_SIT_TERMINAL_LINE", "HeaderId"),
        ("RII_SRT_HEADER", "RII_SRT_LINE", "HeaderId"),
        ("RII_SRT_HEADER", "RII_SRT_IMPORT_LINE", "HeaderId"),
        ("RII_SRT_HEADER", "RII_SRT_TERMINAL_LINE", "HeaderId"),
        ("RII_WI_HEADER", "RII_WI_LINE", "HeaderId"),
        ("RII_WI_HEADER", "RII_WI_IMPORT_LINE", "HeaderId"),
        ("RII_WI_HEADER", "RII_WI_TERMINAL_LINE", "HeaderId"),
        ("RII_WO_HEADER", "RII_WO_LINE", "HeaderId"),
        ("RII_WO_HEADER", "RII_WO_IMPORT_LINE", "HeaderId"),
        ("RII_WO_HEADER", "RII_WO_TERMINAL_LINE", "HeaderId"),
        ("RII_WT_HEADER", "RII_WT_LINE", "HeaderId"),
        ("RII_WT_HEADER", "RII_WT_IMPORT_LINE", "HeaderId"),
        ("RII_WT_HEADER", "RII_WT_TERMINAL_LINE", "HeaderId")
    };

    private static readonly (string HeaderTable, string ImportLineTable, string RouteTable)[] RouteBackfillTables =
    {
        ("RII_GR_HEADER", "RII_GR_IMPORT_LINE", "RII_GR_ROUTE"),
        ("RII_IC_HEADER", "RII_IC_IMPORT_LINE", "RII_IC_ROUTE"),
        ("RII_PR_HEADER", "RII_PR_IMPORT_LINE", "RII_PR_ROUTE"),
        ("RII_PT_HEADER", "RII_PT_IMPORT_LINE", "RII_PT_ROUTE"),
        ("RII_SH_HEADER", "RII_SH_IMPORT_LINE", "RII_SH_ROUTE"),
        ("RII_SIT_HEADER", "RII_SIT_IMPORT_LINE", "RII_SIT_ROUTE"),
        ("RII_SRT_HEADER", "RII_SRT_IMPORT_LINE", "RII_SRT_ROUTE"),
        ("RII_WI_HEADER", "RII_WI_IMPORT_LINE", "RII_WI_ROUTE"),
        ("RII_WO_HEADER", "RII_WO_IMPORT_LINE", "RII_WO_ROUTE"),
        ("RII_WT_HEADER", "RII_WT_IMPORT_LINE", "RII_WT_ROUTE")
    };

    private static readonly (string HeaderTable, string LineTable, string LineSerialTable, string LineIdColumn)[] LineSerialBackfillTables =
    {
        ("RII_GR_HEADER", "RII_GR_LINE", "RII_GR_LINE_SERIAL", "LineId"),
        ("RII_PR_HEADER", "RII_PR_LINE", "RII_PR_LINE_SERIAL", "LineId"),
        ("RII_PT_HEADER", "RII_PT_LINE", "RII_PT_LINE_SERIAL", "LineId"),
        ("RII_SH_HEADER", "RII_SH_LINE", "RII_SH_LINE_SERIAL", "LineId"),
        ("RII_SIT_HEADER", "RII_SIT_LINE", "RII_SIT_LINE_SERIAL", "LineId"),
        ("RII_SRT_HEADER", "RII_SRT_LINE", "RII_SRT_LINE_SERIAL", "LineId"),
        ("RII_WI_HEADER", "RII_WI_LINE", "RII_WI_LINE_SERIAL", "LineId"),
        ("RII_WO_HEADER", "RII_WO_LINE", "RII_WO_LINE_SERIAL", "LineId"),
        ("RII_WT_HEADER", "RII_WT_LINE", "RII_WT_LINE_SERIAL", "LineId")
    };
}
