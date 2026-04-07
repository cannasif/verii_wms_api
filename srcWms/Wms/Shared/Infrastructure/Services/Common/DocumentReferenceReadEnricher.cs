using Microsoft.EntityFrameworkCore;
using Wms.Application.Common;
using Wms.Domain.Common;
using CustomerEntity = Wms.Domain.Entities.Customer.Customer;
using StockEntity = Wms.Domain.Entities.Stock.Stock;
using WarehouseEntity = Wms.Domain.Entities.Warehouse.Warehouse;
using YapKodEntity = Wms.Domain.Entities.YapKod.YapKod;

namespace Wms.Infrastructure.Services.Common;

public sealed class DocumentReferenceReadEnricher : IDocumentReferenceReadEnricher
{
    private readonly IRepository<CustomerEntity> _customers;
    private readonly IRepository<StockEntity> _stocks;
    private readonly IRepository<WarehouseEntity> _warehouses;
    private readonly IRepository<YapKodEntity> _yapKodlar;

    public DocumentReferenceReadEnricher(
        IRepository<CustomerEntity> customers,
        IRepository<StockEntity> stocks,
        IRepository<WarehouseEntity> warehouses,
        IRepository<YapKodEntity> yapKodlar)
    {
        _customers = customers;
        _stocks = stocks;
        _warehouses = warehouses;
        _yapKodlar = yapKodlar;
    }

    public async Task EnrichHeadersAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class
    {
        if (items.Count == 0) return;

        var customerIds = items.Select(x => GetLong(x, "CustomerId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var sourceWarehouseIds = items.Select(x => GetLong(x, "SourceWarehouseId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var targetWarehouseIds = items.Select(x => GetLong(x, "TargetWarehouseId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();

        var customerMap = await _customers.Query().Where(x => customerIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
        var warehouseIds = sourceWarehouseIds.Concat(targetWarehouseIds).Distinct().ToArray();
        var warehouseMap = await _warehouses.Query().Where(x => warehouseIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var item in items)
        {
            var customerId = GetLong(item, "CustomerId");
            if (customerId.HasValue && customerMap.TryGetValue(customerId.Value, out var customer))
            {
                SetString(item, "CustomerCode", customer.CustomerCode);
                SetString(item, "CustomerName", customer.CustomerName);
            }

            var sourceWarehouseId = GetLong(item, "SourceWarehouseId");
            if (sourceWarehouseId.HasValue && warehouseMap.TryGetValue(sourceWarehouseId.Value, out var sourceWarehouse))
            {
                SetString(item, "SourceWarehouse", sourceWarehouse.WarehouseCode.ToString());
                SetString(item, "SourceWarehouseName", sourceWarehouse.WarehouseName);
            }

            var targetWarehouseId = GetLong(item, "TargetWarehouseId");
            if (targetWarehouseId.HasValue && warehouseMap.TryGetValue(targetWarehouseId.Value, out var targetWarehouse))
            {
                SetString(item, "TargetWarehouse", targetWarehouse.WarehouseCode.ToString());
                SetString(item, "TargetWarehouseName", targetWarehouse.WarehouseName);
            }
        }
    }

    public async Task EnrichLinesAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class
    {
        if (items.Count == 0) return;

        var stockIds = items.Select(x => GetLong(x, "StockId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var yapKodIds = items.Select(x => GetLong(x, "YapKodId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();

        var stockMap = await _stocks.Query().Where(x => stockIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
        var yapKodMap = await _yapKodlar.Query().Where(x => yapKodIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var item in items)
        {
            var stockId = GetLong(item, "StockId");
            if (stockId.HasValue && stockMap.TryGetValue(stockId.Value, out var stock))
            {
                SetString(item, "StockCode", stock.ErpStockCode);
                SetString(item, "StockName", stock.StockName);
                if (string.IsNullOrWhiteSpace(GetString(item, "Unit")))
                {
                    SetString(item, "Unit", stock.Unit);
                }
            }

            var yapKodId = GetLong(item, "YapKodId");
            if (yapKodId.HasValue && yapKodMap.TryGetValue(yapKodId.Value, out var yapKod))
            {
                SetString(item, "YapKod", yapKod.YapKodCode);
                SetString(item, "YapAcik", yapKod.YapAcik);
            }
        }
    }

    public async Task EnrichImportLinesAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class
    {
        await EnrichLinesAsync(items, cancellationToken);
    }

    public async Task EnrichLineSerialsAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class
    {
        if (items.Count == 0) return;

        var sourceWarehouseIds = items.Select(x => GetLong(x, "SourceWarehouseId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var targetWarehouseIds = items.Select(x => GetLong(x, "TargetWarehouseId")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var warehouseIds = sourceWarehouseIds.Concat(targetWarehouseIds).Distinct().ToArray();
        var warehouseMap = await _warehouses.Query().Where(x => warehouseIds.Contains(x.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var item in items)
        {
            var sourceWarehouseId = GetLong(item, "SourceWarehouseId");
            if (sourceWarehouseId.HasValue && warehouseMap.TryGetValue(sourceWarehouseId.Value, out var sourceWarehouse))
            {
                SetString(item, "SourceWarehouseName", sourceWarehouse.WarehouseName);
            }

            var targetWarehouseId = GetLong(item, "TargetWarehouseId");
            if (targetWarehouseId.HasValue && warehouseMap.TryGetValue(targetWarehouseId.Value, out var targetWarehouse))
            {
                SetString(item, "TargetWarehouseName", targetWarehouse.WarehouseName);
            }
        }
    }

    public async Task EnrichRoutesAsync<T>(IList<T> items, CancellationToken cancellationToken = default) where T : class
    {
        if (items.Count == 0) return;

        var sourceWarehouseCodes = items.Select(x => GetInt(x, "SourceWarehouse")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var targetWarehouseCodes = items.Select(x => GetInt(x, "TargetWarehouse")).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        var warehouseCodes = sourceWarehouseCodes.Concat(targetWarehouseCodes).Distinct().ToArray();
        var warehouseMap = await _warehouses.Query()
            .Where(x => warehouseCodes.Contains(x.WarehouseCode))
            .ToDictionaryAsync(x => x.WarehouseCode, cancellationToken);

        foreach (var item in items)
        {
            var sourceWarehouseCode = GetInt(item, "SourceWarehouse");
            if (sourceWarehouseCode.HasValue && warehouseMap.TryGetValue(sourceWarehouseCode.Value, out var sourceWarehouse))
            {
                SetString(item, "SourceWarehouseName", sourceWarehouse.WarehouseName);
            }

            var targetWarehouseCode = GetInt(item, "TargetWarehouse");
            if (targetWarehouseCode.HasValue && warehouseMap.TryGetValue(targetWarehouseCode.Value, out var targetWarehouse))
            {
                SetString(item, "TargetWarehouseName", targetWarehouse.WarehouseName);
            }
        }
    }

    private static long? GetLong<T>(T item, string propertyName) where T : class
    {
        var value = item.GetType().GetProperty(propertyName)?.GetValue(item);
        return value switch
        {
            long longValue => longValue,
            int intValue => intValue,
            _ => null
        };
    }

    private static string? GetString<T>(T item, string propertyName) where T : class
    {
        return item.GetType().GetProperty(propertyName)?.GetValue(item) as string;
    }

    private static int? GetInt<T>(T item, string propertyName) where T : class
    {
        var value = item.GetType().GetProperty(propertyName)?.GetValue(item);
        return value switch
        {
            int intValue => intValue,
            long longValue when longValue >= int.MinValue && longValue <= int.MaxValue => (int)longValue,
            _ => null
        };
    }

    private static void SetString<T>(T item, string propertyName, string? value) where T : class
    {
        var property = item.GetType().GetProperty(propertyName);
        if (property?.CanWrite == true && property.PropertyType == typeof(string))
        {
            property.SetValue(item, value);
        }
    }
}
