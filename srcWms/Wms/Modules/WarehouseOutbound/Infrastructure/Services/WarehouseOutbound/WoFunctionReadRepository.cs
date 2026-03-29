using Microsoft.EntityFrameworkCore;
using Wms.Application.WarehouseOutbound.Services;
using Wms.Domain.Entities.WarehouseOutbound.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.WarehouseOutbound;

public sealed class WoFunctionReadRepository : IWoFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public WoFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnWoOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnWoOpenOrderHeader>("SELECT * FROM dbo.RII_FN_WO_HEADER({0}, {1})", customerCode, branchCode).ToListAsync(cancellationToken);
    }

    public Task<List<FnWoOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnWoOpenOrderLine>("SELECT * FROM dbo.RII_FN_WO_LINE({0}, {1})", siparisNoCsv, branchCode).ToListAsync(cancellationToken);
    }
}
