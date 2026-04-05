using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Wms.Application.Common;
using Wms.Domain.Common;
using CustomerEntity = Wms.Domain.Entities.Customer.Customer;
using StockEntity = Wms.Domain.Entities.Stock.Stock;
using WarehouseEntity = Wms.Domain.Entities.Warehouse.Warehouse;
using YapKodEntity = Wms.Domain.Entities.YapKod.YapKod;

namespace Wms.Infrastructure.Services.Common;

public sealed class EntityReferenceResolver : IEntityReferenceResolver
{
    private static readonly BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

    private readonly IRepository<CustomerEntity> _customers;
    private readonly IRepository<StockEntity> _stocks;
    private readonly IRepository<WarehouseEntity> _warehouses;
    private readonly IRepository<YapKodEntity> _yapKodlar;

    public EntityReferenceResolver(
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

    public async Task ResolveAsync(object entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await ResolveCustomerAsync(entity, cancellationToken);
        await ResolveStockAsync(entity, cancellationToken);
        await ResolveYapKodAsync(entity, cancellationToken);
        await ResolveWarehouseAsync(entity, "WarehouseId", "WarehouseCode", cancellationToken);
        await ResolveWarehouseAsync(entity, "SourceWarehouseId", "SourceWarehouse", cancellationToken);
        await ResolveWarehouseAsync(entity, "TargetWarehouseId", "TargetWarehouse", cancellationToken);
    }

    public async Task ResolveManyAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            if (entity != null)
            {
                await ResolveAsync(entity, cancellationToken);
            }
        }
    }

    private async Task ResolveCustomerAsync(object entity, CancellationToken cancellationToken)
    {
        var idProp = entity.GetType().GetProperty("CustomerId", Flags);
        var codeProp = entity.GetType().GetProperty("CustomerCode", Flags);
        if (idProp == null || codeProp == null || !codeProp.CanWrite)
        {
            return;
        }

        var customerId = GetNullableLong(idProp, entity);
        var customerCode = GetNullableString(codeProp, entity);

        if (customerId.HasValue)
        {
            var customer = await _customers.Query().FirstOrDefaultAsync(x => x.Id == customerId.Value && !x.IsDeleted, cancellationToken);
            if (customer != null)
            {
                codeProp.SetValue(entity, customer.CustomerCode);
            }
            return;
        }

        if (!string.IsNullOrWhiteSpace(customerCode) && idProp.CanWrite)
        {
            var customer = await _customers.Query().FirstOrDefaultAsync(x => x.CustomerCode == customerCode && !x.IsDeleted, cancellationToken);
            if (customer != null)
            {
                idProp.SetValue(entity, customer.Id);
            }
        }
    }

    private async Task ResolveStockAsync(object entity, CancellationToken cancellationToken)
    {
        var idProp = entity.GetType().GetProperty("StockId", Flags);
        var codeProp = entity.GetType().GetProperty("StockCode", Flags);
        if (idProp == null || codeProp == null || !codeProp.CanWrite)
        {
            return;
        }

        var stockId = GetNullableLong(idProp, entity);
        var stockCode = GetNullableString(codeProp, entity);

        if (stockId.HasValue)
        {
            var stock = await _stocks.Query().FirstOrDefaultAsync(x => x.Id == stockId.Value && !x.IsDeleted, cancellationToken);
            if (stock != null)
            {
                codeProp.SetValue(entity, stock.ErpStockCode);
            }
            return;
        }

        if (!string.IsNullOrWhiteSpace(stockCode) && idProp.CanWrite)
        {
            var stock = await _stocks.Query().FirstOrDefaultAsync(x => x.ErpStockCode == stockCode && !x.IsDeleted, cancellationToken);
            if (stock != null)
            {
                idProp.SetValue(entity, stock.Id);
            }
        }
    }

    private async Task ResolveYapKodAsync(object entity, CancellationToken cancellationToken)
    {
        var idProp = entity.GetType().GetProperty("YapKodId", Flags);
        var codeProp = entity.GetType().GetProperty("YapKod", Flags);
        if (idProp == null || codeProp == null || !codeProp.CanWrite)
        {
            return;
        }

        var yapKodId = GetNullableLong(idProp, entity);
        var yapKod = GetNullableString(codeProp, entity);

        if (yapKodId.HasValue)
        {
            var entityRow = await _yapKodlar.Query().FirstOrDefaultAsync(x => x.Id == yapKodId.Value && !x.IsDeleted, cancellationToken);
            if (entityRow != null)
            {
                codeProp.SetValue(entity, entityRow.YapKodCode);
            }
            return;
        }

        if (!string.IsNullOrWhiteSpace(yapKod) && idProp.CanWrite)
        {
            var entityRow = await _yapKodlar.Query().FirstOrDefaultAsync(x => x.YapKodCode == yapKod && !x.IsDeleted, cancellationToken);
            if (entityRow != null)
            {
                idProp.SetValue(entity, entityRow.Id);
            }
        }
    }

    private async Task ResolveWarehouseAsync(object entity, string idPropertyName, string codePropertyName, CancellationToken cancellationToken)
    {
        var idProp = entity.GetType().GetProperty(idPropertyName, Flags);
        var codeProp = entity.GetType().GetProperty(codePropertyName, Flags);
        if (idProp == null || codeProp == null || !codeProp.CanWrite)
        {
            return;
        }

        var warehouseId = GetNullableLong(idProp, entity);
        var warehouseCode = GetNullableString(codeProp, entity);

        if (warehouseId.HasValue)
        {
            var warehouse = await _warehouses.Query().FirstOrDefaultAsync(x => x.Id == warehouseId.Value && !x.IsDeleted, cancellationToken);
            if (warehouse != null)
            {
                codeProp.SetValue(entity, warehouse.WarehouseCode.ToString());
            }
            return;
        }

        if (!string.IsNullOrWhiteSpace(warehouseCode) && idProp.CanWrite && int.TryParse(warehouseCode, out var parsedWarehouseCode))
        {
            var warehouse = await _warehouses.Query().FirstOrDefaultAsync(x => x.WarehouseCode == parsedWarehouseCode && !x.IsDeleted, cancellationToken);
            if (warehouse != null)
            {
                idProp.SetValue(entity, warehouse.Id);
            }
        }
    }

    private static long? GetNullableLong(PropertyInfo property, object entity)
    {
        var value = property.GetValue(entity);
        if (value == null)
        {
            return null;
        }

        if (value is long longValue)
        {
            return longValue;
        }

        if (value is int intValue)
        {
            return intValue;
        }

        return null;
    }

    private static string? GetNullableString(PropertyInfo property, object entity)
    {
        return property.GetValue(entity)?.ToString();
    }
}
