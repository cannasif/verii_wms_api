using Wms.Application.Common;
using Wms.Application.WarehouseInbound.Dtos;

namespace Wms.Application.WarehouseInbound.Services;

public interface IWiLineSerialService
{
    Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<WiLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<WiLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiLineSerialDto>> CreateAsync(CreateWiLineSerialDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<WiLineSerialDto>> UpdateAsync(long id, UpdateWiLineSerialDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
