using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class UpdateGrHeaderUseCase : IUpdateGrHeaderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public UpdateGrHeaderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<GrHeaderDto>> ExecuteAsync(int id, UpdateGrHeaderDto updateDto, CancellationToken cancellationToken = default)
        {
            return _grHeaderService.UpdateAsync(id, updateDto, cancellationToken);
        }
    }
}

