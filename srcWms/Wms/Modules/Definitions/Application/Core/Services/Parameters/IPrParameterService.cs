using Wms.Application.Common;
using Wms.Application.Definitions.Dtos.Parameters;

namespace Wms.Application.Definitions.Services.Parameters;

public interface IPrParameterService
{
    Task<ApiResponse<IEnumerable<PrParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PrParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrParameterDto>> CreateAsync(CreatePrParameterDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrParameterDto>> UpdateAsync(long id, UpdatePrParameterDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
