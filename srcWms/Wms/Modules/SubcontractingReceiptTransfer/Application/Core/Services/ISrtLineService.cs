using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;

namespace Wms.Application.SubcontractingReceiptTransfer.Services;

public interface ISrtLineService
{
    Task<ApiResponse<IEnumerable<SrtLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SrtLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtLineDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SrtLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtLineDto>> CreateAsync(CreateSrtLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SrtLineDto>> UpdateAsync(long id, UpdateSrtLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
