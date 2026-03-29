using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;

namespace Wms.Application.Shipping.Services;

public interface IShLineSerialService
{
    Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<ShLineSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShLineSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<ShLineSerialDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShLineSerialDto>> CreateAsync(CreateShLineSerialDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<ShLineSerialDto>> UpdateAsync(long id, UpdateShLineSerialDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
