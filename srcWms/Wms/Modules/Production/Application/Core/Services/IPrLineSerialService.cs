using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public interface IPrLineSerialService
{
    Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PrLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrLineSerialDto>> CreateAsync(CreatePrLineSerialDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrLineSerialDto>> UpdateAsync(long id, UpdatePrLineSerialDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
