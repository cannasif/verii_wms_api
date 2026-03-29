using Microsoft.EntityFrameworkCore;
using Wms.Application.GoodsReceipt.Services;
using Wms.Domain.Entities.GoodsReceipt.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.GoodsReceipt;

public sealed class GoodsReceiptOpenOrderReadRepository : IGoodsReceiptOpenOrderReadRepository
{
    private readonly WmsDbContext _dbContext;

    public GoodsReceiptOpenOrderReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnGoodsOpenOrdersHeader>> GetOpenOrderHeadersAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database
            .SqlQueryRaw<FnGoodsOpenOrdersHeader>(
                "SELECT * FROM dbo.RII_FN_GR_OPENORDERS_HEADER({0}, {1})",
                customerCode,
                branchCode)
            .ToListAsync(cancellationToken);
    }

    public Task<List<FnGoodsOpenOrdersLine>> GetOpenOrderLinesAsync(string ordersCsv, string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database
            .SqlQueryRaw<FnGoodsOpenOrdersLine>(
                "SELECT * FROM dbo.RII_FN_GR_OPENORDERS_LINE({0}, {1}, {2})",
                ordersCsv,
                customerCode,
                branchCode)
            .ToListAsync(cancellationToken);
    }
}
