namespace Wms.Application.Common;

public interface IErpReadEnrichmentService
{
    Task<ApiResponse<IEnumerable<T>>> PopulateCustomerNamesAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> items, CancellationToken cancellationToken = default);
}
