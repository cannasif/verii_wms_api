using Microsoft.EntityFrameworkCore;
using Wms.Application.Shipping.Services;
using Wms.Domain.Entities.Shipping.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.Shipping;

public sealed class ShFunctionReadRepository : IShFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public ShFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnTransferOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnTransferOpenOrderHeader>("SELECT * FROM dbo.RII_FN_SH_HEADER({0}, {1})", customerCode, branchCode).ToListAsync(cancellationToken);
    }

    public Task<List<FnTransferOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnTransferOpenOrderLine>("SELECT * FROM dbo.RII_FN_SH_LINE({0}, {1})", siparisNoCsv, branchCode).ToListAsync(cancellationToken);
    }
}
