using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IGenerateGoodReceiptOrderUseCase
    {
        Task<ApiResponse<GrHeaderDto>> ExecuteAsync(GenerateGoodReceiptOrderRequestDto request, CancellationToken cancellationToken = default);
    }
}

