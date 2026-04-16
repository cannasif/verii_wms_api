using Wms.Application.Common;
using Wms.Application.ServiceAllocation.Dtos;

namespace Wms.Application.ServiceAllocation.Services;

public interface IBusinessDocumentLinkService
{
    Task<ApiResponse<IEnumerable<BusinessDocumentLinkDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<BusinessDocumentLinkDto>>> GetByBusinessEntityAsync(long businessEntityId, Wms.Domain.Entities.ServiceAllocation.Enums.BusinessEntityType businessEntityType, CancellationToken cancellationToken = default);
    Task<ApiResponse<PagedResponse<BusinessDocumentLinkDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<BusinessDocumentLinkDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ApiResponse<BusinessDocumentLinkDto>> CreateAsync(CreateBusinessDocumentLinkDto createDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<BusinessDocumentLinkDto>> UpdateAsync(long id, UpdateBusinessDocumentLinkDto updateDto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
