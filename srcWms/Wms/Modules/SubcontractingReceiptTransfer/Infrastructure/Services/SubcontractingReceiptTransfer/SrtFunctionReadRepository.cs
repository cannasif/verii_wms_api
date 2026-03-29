using Microsoft.EntityFrameworkCore;
using Wms.Application.SubcontractingReceiptTransfer.Services;
using Wms.Domain.Entities.SubcontractingReceiptTransfer.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.SubcontractingReceiptTransfer;

public sealed class SrtFunctionReadRepository : ISrtFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public SrtFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnSrtOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnSrtOpenOrderHeader>("SELECT * FROM dbo.RII_FN_SRT_HEADER({0}, {1})", customerCode, branchCode).ToListAsync(cancellationToken);
    }

    public Task<List<FnSrtOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnSrtOpenOrderLine>("SELECT * FROM dbo.RII_FN_SRT_LINE({0}, {1})", siparisNoCsv, branchCode).ToListAsync(cancellationToken);
    }
}
