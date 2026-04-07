using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;

namespace Wms.Application.InventoryCount.Services;

public interface IIcCountEntryService
{
    Task<ApiResponse<IEnumerable<IcCountEntryDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IcCountEntryDto>> CreateAsync(CreateIcCountEntryDto createDto, CancellationToken cancellationToken = default);
}
