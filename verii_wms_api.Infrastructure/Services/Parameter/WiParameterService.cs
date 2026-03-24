using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WiParameterService : IWiParameterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WiParameterService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        public async Task<ApiResponse<IEnumerable<WiParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entities = await _unitOfWork.WiParameters.GetAllAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<WiParameterDto>>(entities);
                return ApiResponse<IEnumerable<WiParameterDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WiParameterDto>>.ErrorResult(_localizationService.GetLocalizedString("Error_GetAll"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WiParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.WiParameters.Query();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync(requestCancellationToken);
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync(requestCancellationToken);

                var dtos = _mapper.Map<List<WiParameterDto>>(entities);
                var result = new PagedResponse<WiParameterDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<WiParameterDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WiParameterDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<WiParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = await _unitOfWork.WiParameters.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null)
                {
                    return ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                var dto = _mapper.Map<WiParameterDto>(entity);
                return ApiResponse<WiParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_GetById"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiParameterDto>> CreateAsync(CreateWiParameterDto createDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = _mapper.Map<WiParameter>(createDto);
                await _unitOfWork.WiParameters.AddAsync(entity, requestCancellationToken);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                var dto = _mapper.Map<WiParameterDto>(entity);
                return ApiResponse<WiParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Create"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WiParameterDto>> UpdateAsync(long id, UpdateWiParameterDto updateDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = await _unitOfWork.WiParameters.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null)
                {
                    return ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                _unitOfWork.WiParameters.Update(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                var dto = _mapper.Map<WiParameterDto>(entity);
                return ApiResponse<WiParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WiParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Update"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var exists = await _unitOfWork.WiParameters.ExistsAsync(id, requestCancellationToken);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                await _unitOfWork.WiParameters.SoftDelete(id, requestCancellationToken);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ParameterDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("Error_Delete"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
