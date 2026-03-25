using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class GenerateGoodReceiptOrderUseCase : IGenerateGoodReceiptOrderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public GenerateGoodReceiptOrderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<GrHeaderDto>> ExecuteAsync(GenerateGoodReceiptOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            return _grHeaderService.GenerateGoodReceiptOrderAsync(request, cancellationToken);
        }
    }
}

