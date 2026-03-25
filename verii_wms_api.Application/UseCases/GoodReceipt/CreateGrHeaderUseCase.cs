using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.UseCases.GoodReceipt
{
    public sealed class CreateGrHeaderUseCase : ICreateGrHeaderUseCase
    {
        private readonly IGrHeaderService _grHeaderService;

        public CreateGrHeaderUseCase(IGrHeaderService grHeaderService)
        {
            _grHeaderService = grHeaderService;
        }

        public Task<ApiResponse<GrHeaderDto>> ExecuteAsync(CreateGrHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            return _grHeaderService.CreateAsync(createDto, cancellationToken);
        }
    }
}

