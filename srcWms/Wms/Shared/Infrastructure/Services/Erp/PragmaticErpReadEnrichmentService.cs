using Wms.Application.Common;

namespace Wms.Infrastructure.Services.Erp;

public sealed class PragmaticErpReadEnrichmentService : IErpReadEnrichmentService
{
    public Task<ApiResponse<IEnumerable<T>>> PopulateCustomerNamesAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ApiResponse<IEnumerable<T>>.SuccessResult(items, "ERP enrichment skipped"));
    }

    public Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ApiResponse<IEnumerable<T>>.SuccessResult(items, "ERP enrichment skipped"));
    }
}
