using Microsoft.EntityFrameworkCore;
using Wms.Application.WarehouseInbound.Services;
using Wms.Domain.Entities.WarehouseInbound.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.WarehouseInbound;

public sealed class WiFunctionReadRepository : IWiFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public WiFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnWiOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnWiOpenOrderHeader>("SELECT * FROM dbo.RII_FN_WI_HEADER({0}, {1})", customerCode, branchCode).ToListAsync(cancellationToken);
    }

    public Task<List<FnWiOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnWiOpenOrderLine>("SELECT * FROM dbo.RII_FN_WI_LINE({0}, {1})", siparisNoCsv, branchCode).ToListAsync(cancellationToken);
    }
}
