using Wms.Application.Common;
using Wms.Application.Production.Dtos;

namespace Wms.Application.Production.Services;

public interface IPrOperationService
{
    Task<ApiResponse<PrOperationDto>> StartAsync(StartPrOperationRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrOperationDto>> PauseAsync(long operationId, PrOperationEventRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrOperationDto>> ResumeAsync(long operationId, PrOperationEventRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrOperationDto>> AddConsumptionAsync(long operationId, AddPrOperationLineRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrOperationDto>> AddOutputAsync(long operationId, AddPrOperationLineRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResponse<PrOperationDto>> CompleteAsync(long operationId, PrOperationEventRequestDto request, CancellationToken cancellationToken = default);
}
