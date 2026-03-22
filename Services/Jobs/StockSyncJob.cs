using Hangfire;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Services.Jobs
{
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 120, 300 })]
    public class StockSyncJob : IStockSyncJob
    {
        private const string RecurringJobId = "erp-stock-sync-job";
        private readonly ErpDbContext _erpDb;
        private readonly WmsDbContext _db;
        private readonly ILogger<StockSyncJob> _logger;

        public StockSyncJob(
            ErpDbContext erpDb,
            WmsDbContext db,
            ILogger<StockSyncJob> logger)
        {
            _erpDb = erpDb;
            _db = db;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Stock sync started.");

            List<RII_VW_STOK> erpStocks;
            try
            {
                erpStocks = await _erpDb.Stoks
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await LogRecordFailureAsync("ERP_FETCH", ex);
                _logger.LogWarning(ex, "Stock sync aborted: ERP fetch failed.");
                return;
            }

            if (erpStocks.Count == 0)
            {
                _logger.LogInformation("Stock sync skipped: no ERP records returned.");
                return;
            }

            var createdCount = 0;
            var updatedCount = 0;
            var failedCount = 0;
            var skippedCount = 0;
            var duplicatePayloadCount = 0;
            var processedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var erpStock in erpStocks)
            {
                var code = erpStock.STOK_KODU?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(code))
                {
                    skippedCount++;
                    continue;
                }

                if (!processedCodes.Add(code))
                {
                    duplicatePayloadCount++;
                    continue;
                }

                try
                {
                    var stock = await _db.Stocks
                        .IgnoreQueryFilters()
                        .FirstOrDefaultAsync(x => x.ErpStockCode == code);

                    var stockName = string.IsNullOrWhiteSpace(erpStock.STOK_ADI) ? code : erpStock.STOK_ADI!.Trim();
                    var unit = EmptyToNull(erpStock.OLCU_BR1);
                    var ureticiKodu = EmptyToNull(erpStock.URETICI_KODU);
                    var grupKodu = EmptyToNull(erpStock.GRUP_KODU);
                    var grupAdi = null as string;
                    var kod1 = EmptyToNull(erpStock.KOD_1);
                    var kod1Adi = null as string;
                    var kod2 = EmptyToNull(erpStock.KOD_2);
                    var kod2Adi = null as string;
                    var kod3 = EmptyToNull(erpStock.KOD_3);
                    var kod3Adi = null as string;
                    var kod4 = EmptyToNull(erpStock.KOD_4);
                    var kod4Adi = null as string;
                    var kod5 = EmptyToNull(erpStock.KOD_5);
                    var kod5Adi = null as string;
                    var branchCode = (int)(erpStock.SUBE_KODU ?? 0);

                    if (stock == null)
                    {
                        _db.Stocks.Add(new Stock
                        {
                            ErpStockCode = code,
                            StockName = stockName,
                            Unit = unit,
                            UreticiKodu = ureticiKodu,
                            GrupKodu = grupKodu,
                            GrupAdi = grupAdi,
                            Kod1 = kod1,
                            Kod1Adi = kod1Adi,
                            Kod2 = kod2,
                            Kod2Adi = kod2Adi,
                            Kod3 = kod3,
                            Kod3Adi = kod3Adi,
                            Kod4 = kod4,
                            Kod4Adi = kod4Adi,
                            Kod5 = kod5,
                            Kod5Adi = kod5Adi,
                            BranchCode = branchCode,
                            LastSyncDate = DateTime.UtcNow,
                            CreatedDate = DateTimeProvider.Now,
                            IsDeleted = false
                        });

                        await _db.SaveChangesAsync();
                        createdCount++;
                        continue;
                    }

                    var updated = false;
                    updated |= SetIfChanged(stock, x => x.StockName, stockName);
                    updated |= SetIfChanged(stock, x => x.Unit, unit);
                    updated |= SetIfChanged(stock, x => x.UreticiKodu, ureticiKodu);
                    updated |= SetIfChanged(stock, x => x.GrupKodu, grupKodu);
                    updated |= SetIfChanged(stock, x => x.GrupAdi, grupAdi);
                    updated |= SetIfChanged(stock, x => x.Kod1, kod1);
                    updated |= SetIfChanged(stock, x => x.Kod1Adi, kod1Adi);
                    updated |= SetIfChanged(stock, x => x.Kod2, kod2);
                    updated |= SetIfChanged(stock, x => x.Kod2Adi, kod2Adi);
                    updated |= SetIfChanged(stock, x => x.Kod3, kod3);
                    updated |= SetIfChanged(stock, x => x.Kod3Adi, kod3Adi);
                    updated |= SetIfChanged(stock, x => x.Kod4, kod4);
                    updated |= SetIfChanged(stock, x => x.Kod4Adi, kod4Adi);
                    updated |= SetIfChanged(stock, x => x.Kod5, kod5);
                    updated |= SetIfChanged(stock, x => x.Kod5Adi, kod5Adi);
                    updated |= SetIfChanged(stock, x => x.BranchCode, branchCode);

                    if (stock.IsDeleted)
                    {
                        stock.IsDeleted = false;
                        stock.DeletedDate = null;
                        stock.DeletedBy = null;
                        updated = true;
                    }

                    if (!updated)
                    {
                        continue;
                    }

                    stock.UpdatedDate = DateTimeProvider.Now;
                    stock.UpdatedBy = null;
                    stock.LastSyncDate = DateTime.UtcNow;

                    await _db.SaveChangesAsync();
                    updatedCount++;
                }
                catch (Exception ex)
                {
                    failedCount++;
                    await LogRecordFailureAsync(code, ex);
                    _db.ChangeTracker.Clear();
                }
            }

            _logger.LogInformation(
                "Stock sync completed. created={Created}, updated={Updated}, failed={Failed}, skipped={Skipped}, duplicatePayload={DuplicatePayload}.",
                createdCount,
                updatedCount,
                failedCount,
                skippedCount,
                duplicatePayloadCount);
        }

        private async Task LogRecordFailureAsync(string code, Exception ex)
        {
            _logger.LogError(ex, "Stock sync record failed. StockCode: {StockCode}", code);

            try
            {
                _db.JobFailureLogs.Add(new JobFailureLog
                {
                    JobId = $"{RecurringJobId}:{code}:{DateTime.UtcNow:yyyyMMddHHmmssfff}",
                    JobName = $"{typeof(StockSyncJob).FullName}.ExecuteAsync",
                    FailedAt = DateTime.UtcNow,
                    Reason = $"StockCode={code}",
                    ExceptionType = ex.GetType().FullName,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace?.Length > 4000 ? ex.StackTrace[..4000] : ex.StackTrace,
                    Queue = "default",
                    RetryCount = 0,
                    CreatedDate = DateTimeProvider.Now,
                    IsDeleted = false
                });

                await _db.SaveChangesAsync();
            }
            catch (Exception logEx)
            {
                _logger.LogWarning(logEx, "Stock sync failure could not be written to RII_JOB_FAILURE_LOG. StockCode: {StockCode}", code);
            }
        }

        private static string? EmptyToNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static bool SetIfChanged<TEntity, TValue>(
            TEntity entity,
            System.Linq.Expressions.Expression<Func<TEntity, TValue>> selector,
            TValue value)
        {
            if (selector.Body is not System.Linq.Expressions.MemberExpression member)
            {
                return false;
            }

            if (member.Member is not System.Reflection.PropertyInfo property)
            {
                return false;
            }

            var current = property.GetValue(entity);
            if (Equals(current, value))
            {
                return false;
            }

            property.SetValue(entity, value);
            return true;
        }
    }
}
