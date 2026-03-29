using Wms.Domain.Entities.ProductionTransfer.Functions;

namespace Wms.Application.ProductionTransfer.Services;

public interface IPtFunctionReadRepository
{
    Task<List<FnProductHeader>> GetProductHeaderRowsAsync(string isemriNo, CancellationToken cancellationToken = default);
    Task<List<FnProductLine>> GetProductLineRowsAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default);
}
