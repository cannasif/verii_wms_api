using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class BulkCreateGrHeaderUseCase : IBulkCreateGrHeaderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public BulkCreateGrHeaderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<long>> ExecuteAsync(BulkCreateGrRequestDto request, CancellationToken cancellationToken = default)
        {
            return _grHeaderService.BulkCreateAsync(request, cancellationToken);
        }
    }
}

