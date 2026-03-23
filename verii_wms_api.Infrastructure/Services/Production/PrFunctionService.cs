using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services
{
    public class PrFunctionService : IPrFunctionService
    {
        private readonly IErpUnitOfWork _erpUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PrFunctionService(IErpUnitOfWork erpUnitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _erpUnitOfWork = erpUnitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo)
        {
            try
            {
                var result = await _erpUnitOfWork.SqlQuery<RII_FN_PRODUCT_HEADER>("SELECT * FROM dbo.RII_FN_PRODUCT_HEADER({0})", isemriNo)
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<ProductHeaderDto>>(result);

                return ApiResponse<List<ProductHeaderDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProductHeaderRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductHeaderDto>>.ErrorResult(_localizationService.GetLocalizedString("ProductHeaderRetrievalError"), ex.Message, 500);
            }
        }

        public async Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null)
        {
            try
            {
                var result = await _erpUnitOfWork.SqlQuery<RII_FN_PRODUCT_LINE>("SELECT * FROM dbo.RII_FN_PRODUCT_LINE({0}, {1}, {2})", isemriNo!, fisNo!, mamulKodu!)
                    .ToListAsync();

                var mappedResult = _mapper.Map<List<ProductLineDto>>(result);

                return ApiResponse<List<ProductLineDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProductLinesRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProductLineDto>>.ErrorResult(_localizationService.GetLocalizedString("ProductLinesRetrievalError"), ex.Message, 500);
            }
        }
    }
}
