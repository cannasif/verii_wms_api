using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddHangfireMonitoringAndErpMirrorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_CUSTOMER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxOffice = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TcknNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    SalesRepCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: false),
                    BusinessUnitCode = table.Column<short>(type: "smallint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsErpIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ErpIntegrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_CUSTOMER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_CUSTOMER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_JOB_FAILURE_LOG",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FailedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ExceptionType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Queue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_JOB_FAILURE_LOG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_JOB_FAILURE_LOG_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_JOB_FAILURE_LOG_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_JOB_FAILURE_LOG_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_STOCK",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErpStockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UreticiKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupKodu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GrupAdi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod1Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod2Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod3Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod4Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Kod5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Kod5Adi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_STOCK", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_STOCK_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_STOCK_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_STOCK_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_CreatedBy",
                table: "RII_CUSTOMER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_CustomerCode",
                table: "RII_CUSTOMER",
                column: "CustomerCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_CustomerName",
                table: "RII_CUSTOMER",
                column: "CustomerName");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_DeletedBy",
                table: "RII_CUSTOMER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_IsDeleted",
                table: "RII_CUSTOMER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_TaxNumber",
                table: "RII_CUSTOMER",
                column: "TaxNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RII_CUSTOMER_UpdatedBy",
                table: "RII_CUSTOMER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobFailureLog_FailedAt",
                table: "RII_JOB_FAILURE_LOG",
                column: "FailedAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobFailureLog_JobId",
                table: "RII_JOB_FAILURE_LOG",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobFailureLog_JobName",
                table: "RII_JOB_FAILURE_LOG",
                column: "JobName");

            migrationBuilder.CreateIndex(
                name: "IX_RII_JOB_FAILURE_LOG_CreatedBy",
                table: "RII_JOB_FAILURE_LOG",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_JOB_FAILURE_LOG_DeletedBy",
                table: "RII_JOB_FAILURE_LOG",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_JOB_FAILURE_LOG_UpdatedBy",
                table: "RII_JOB_FAILURE_LOG",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_CreatedBy",
                table: "RII_STOCK",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_DeletedBy",
                table: "RII_STOCK",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_ErpStockCode",
                table: "RII_STOCK",
                column: "ErpStockCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_IsDeleted",
                table: "RII_STOCK",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_StockName",
                table: "RII_STOCK",
                column: "StockName");

            migrationBuilder.CreateIndex(
                name: "IX_RII_STOCK_UpdatedBy",
                table: "RII_STOCK",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_CUSTOMER");

            migrationBuilder.DropTable(
                name: "RII_JOB_FAILURE_LOG");

            migrationBuilder.DropTable(
                name: "RII_STOCK");
        }
    }
}
