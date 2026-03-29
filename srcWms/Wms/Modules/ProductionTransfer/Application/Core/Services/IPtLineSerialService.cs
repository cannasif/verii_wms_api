using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;

namespace Wms.Application.ProductionTransfer.Services;

public interface IPtLineSerialService
{
    Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PtLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PtLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtLineSerialDto>> CreateAsync(CreatePtLineSerialDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PtLineSerialDto>> UpdateAsync(long id, UpdatePtLineSerialDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
