using Wms.Domain.Entities.GoodsReceipt.Functions;

namespace Wms.Application.GoodsReceipt.Services;

public interface IGoodsReceiptOpenOrderReadRepository
{
    Task<List<FnGoodsOpenOrdersHeader>> GetOpenOrderHeadersAsync(string customerCode, string branchCode, CancellationToken cancellationToken = default);
    Task<List<FnGoodsOpenOrdersLine>> GetOpenOrderLinesAsync(string ordersCsv, string customerCode, string branchCode, CancellationToken cancellationToken = default);
}
