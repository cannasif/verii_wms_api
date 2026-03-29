using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;

namespace Wms.Application.SubcontractingIssueTransfer.Services;

public interface ISitImportLineService
{
    Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<SitImportLineDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitImportLineDto?>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitImportLineDto>>> GetByLineIdAsync(long lineId, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitImportLineDto>> CreateAsync(CreateSitImportLineDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitImportLineDto>> UpdateAsync(long id, UpdateSitImportLineDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<SitImportLineDto>> AddBarcodeBasedonAssignedOrderAsync(AddSitImportBarcodeRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<SitImportLineWithRoutesDto>>> GetCollectedBarcodesByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
}
