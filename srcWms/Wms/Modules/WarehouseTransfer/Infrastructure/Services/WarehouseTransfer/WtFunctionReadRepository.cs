using Microsoft.EntityFrameworkCore;
using Wms.Application.WarehouseTransfer.Services;
using Wms.Domain.Entities.WarehouseTransfer.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.WarehouseTransfer;

public sealed class WtFunctionReadRepository : IWtFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public WtFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnTransferOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnTransferOpenOrderHeader>("SELECT * FROM dbo.RII_FN_WT_HEADER({0}, {1})", customerCode, branchCode).ToListAsync(cancellationToken);
    }

    public Task<List<FnTransferOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnTransferOpenOrderLine>("SELECT * FROM dbo.RII_FN_WT_LINE({0}, {1})", siparisNoCsv, branchCode).ToListAsync(cancellationToken);
    }
}
