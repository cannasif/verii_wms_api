using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public interface IPrImportLineService
{
    Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<PrImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrImportLineDto>> CreateAsync(CreatePrImportLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrImportLineDto>> UpdateAsync(long id, UpdatePrImportLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddPrImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<PrImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
}
