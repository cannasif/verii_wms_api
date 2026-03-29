using Microsoft.EntityFrameworkCore;
using Wms.Application.Production.Services;
using Wms.Domain.Entities.Production.Functions;
using Wms.Infrastructure.Persistence.Context;

namespace Wms.Infrastructure.Services.Production;

public sealed class PrFunctionReadRepository : IPrFunctionReadRepository
{
    private readonly WmsDbContext _dbContext;

    public PrFunctionReadRepository(WmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<FnProductHeader>> GetProductHeaderRowsAsync(string isemriNo, CancellationToken cancellationToken = default)
        => _dbContext.Database.SqlQueryRaw<FnProductHeader>("SELECT * FROM dbo.RII_FN_PRODUCT_HEADER({0})", isemriNo).ToListAsync(cancellationToken);

    public Task<List<FnProductLine>> GetProductLineRowsAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default)
        => _dbContext.Database.SqlQueryRaw<FnProductLine>("SELECT * FROM dbo.RII_FN_PRODUCT_LINE({0}, {1}, {2})", isemriNo ?? string.Empty, fisNo ?? string.Empty, mamulKodu ?? string.Empty).ToListAsync(cancellationToken);
}
