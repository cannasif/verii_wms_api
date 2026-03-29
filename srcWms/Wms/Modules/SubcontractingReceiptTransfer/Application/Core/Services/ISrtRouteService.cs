using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public interface ISrtRouteService
{
    Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SrtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SrtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtRouteDto>> CreateAsync(CreateSrtRouteDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtRouteDto>> UpdateAsync(long id, UpdateSrtRouteDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
