using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class WtParameterService : IWtParameterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WtParameterService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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

        public async Task<ApiResponse<IEnumerable<WtParameterDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entities = await _unitOfWork.WtParameters.GetAllAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<WtParameterDto>>(entities);
                return ApiResponse<IEnumerable<WtParameterDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtParameterDto>>.ErrorResult(_localizationService.GetLocalizedString("Error_GetAll"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WtParameterDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.WtParameters.Query();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync(requestCancellationToken);
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync(requestCancellationToken);

                var dtos = _mapper.Map<List<WtParameterDto>>(entities);
                var result = new PagedResponse<WtParameterDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<WtParameterDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WtParameterDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<WtParameterDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = await _unitOfWork.WtParameters.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null)
                {
                    return ApiResponse<WtParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                var dto = _mapper.Map<WtParameterDto>(entity);
                return ApiResponse<WtParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_GetById"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtParameterDto>> CreateAsync(CreateWtParameterDto createDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = _mapper.Map<WtParameter>(createDto);
                await _unitOfWork.WtParameters.AddAsync(entity, requestCancellationToken);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                var dto = _mapper.Map<WtParameterDto>(entity);
                return ApiResponse<WtParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Create"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtParameterDto>> UpdateAsync(long id, UpdateWtParameterDto updateDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var entity = await _unitOfWork.WtParameters.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null)
                {
                    return ApiResponse<WtParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                _unitOfWork.WtParameters.Update(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                var dto = _mapper.Map<WtParameterDto>(entity);
                return ApiResponse<WtParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Update"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestCancellationToken = ResolveCancellationToken(cancellationToken);
                var exists = await _unitOfWork.WtParameters.ExistsAsync(id, requestCancellationToken);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                await _unitOfWork.WtParameters.SoftDelete(id, requestCancellationToken);
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

