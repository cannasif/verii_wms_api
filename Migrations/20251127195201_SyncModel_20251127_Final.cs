using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20251127_Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_WT_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_WT_ROUTE");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_WO_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_WO_ROUTE");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_WI_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_WI_ROUTE");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_SRT_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_SRT_ROUTE");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_SIT_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_SIT_ROUTE");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_PT_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_PT_ROUTE");

            migrationBuilder.DropColumn(
                name: "RouteCode",
                table: "RII_GR_ROUTE");

            migrationBuilder.DropColumn(
                name: "StockCode",
                table: "RII_GR_ROUTE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_WT_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_WT_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_WO_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_WO_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_WI_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_WI_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_SRT_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_SRT_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_SIT_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_SIT_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_PT_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_PT_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteCode",
                table: "RII_GR_ROUTE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockCode",
                table: "RII_GR_ROUTE",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: true);
        }
    }
}
