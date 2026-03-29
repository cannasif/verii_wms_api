using Wms.Application.Common; using Wms.Application.Production.Dtos;
namespace Wms.Application.Production.Services;
public interface IPrHeaderSerialService
{
Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetAllAsync(CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<PrHeaderSerialDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
Task<ApiResponse<PrHeaderSerialDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default);
Task<ApiResponse<IEnumerable<PrHeaderSerialDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default);
Task<ApiResponse<PrHeaderSerialDto>> CreateAsync(CreatePrHeaderSerialDto dto, CancellationToken cancellationToken = default);
Task<ApiResponse<PrHeaderSerialDto>> UpdateAsync(long id, UpdatePrHeaderSerialDto dto, CancellationToken cancellationToken = default);
Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default);
}
