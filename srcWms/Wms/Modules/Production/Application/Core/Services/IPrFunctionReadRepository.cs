using Wms.Domain.Entities.Production.Functions;

namespace Wms.Application.Production.Services;

public interface IPrFunctionReadRepository
{
    Task<List<FnProductHeader>> GetProductHeaderRowsAsync(string isemriNo, CancellationToken cancellationToken = default);
    Task<List<FnProductLine>> GetProductLineRowsAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default);
}
