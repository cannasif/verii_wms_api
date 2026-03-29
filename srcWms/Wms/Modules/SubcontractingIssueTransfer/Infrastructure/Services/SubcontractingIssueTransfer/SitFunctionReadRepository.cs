using Microsoft.EntityFrameworkCore;
using Wms.Application.SubcontractingIssueTransfer.Services;
using Wms.Domain.Entities.SubcontractingIssueTransfer.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.SubcontractingIssueTransfer;

public sealed class SitFunctionReadRepository : ISitFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public SitFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnSitOpenOrderHeader>> GetHeaderRowsAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnSitOpenOrderHeader>("SELECT * FROM dbo.RII_FN_SIT_HEADER({0}, {1})", customerCode, branchCode).ToListAsync(cancellationToken);
    }

    public Task<List<FnSitOpenOrderLine>> GetLineRowsAsync(string siparisNoCsv, string branchCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.SqlQueryRaw<FnSitOpenOrderLine>("SELECT * FROM dbo.RII_FN_SIT_LINE({0}, {1})", siparisNoCsv, branchCode).ToListAsync(cancellationToken);
    }
}
