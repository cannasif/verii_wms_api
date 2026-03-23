using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class PParameterService : IPParameterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public PParameterService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<PParameterDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.PParameters.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<PParameterDto>>(entities);
                return ApiResponse<IEnumerable<PParameterDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PParameterDto>>.ErrorResult(_localizationService.GetLocalizedString("Error_GetAll"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<PParameterDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.PParameters.Query();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<PParameterDto>>(entities);
                var result = new PagedResponse<PParameterDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<PParameterDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<PParameterDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<PParameterDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.PParameters.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return ApiResponse<PParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                var dto = _mapper.Map<PParameterDto>(entity);
                return ApiResponse<PParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_GetById"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PParameterDto>> CreateAsync(CreatePParameterDto createDto)
        {
            try
            {
                var entity = _mapper.Map<PParameter>(createDto);
                await _unitOfWork.PParameters.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PParameterDto>(entity);
                return ApiResponse<PParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Create"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PParameterDto>> UpdateAsync(long id, UpdatePParameterDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.PParameters.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null)
                {
                    return ApiResponse<PParameterDto>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                _unitOfWork.PParameters.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<PParameterDto>(entity);
                return ApiResponse<PParameterDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ParameterUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PParameterDto>.ErrorResult(_localizationService.GetLocalizedString("Error_Update"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.PParameters.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ParameterNotFound"), _localizationService.GetLocalizedString("ParameterNotFound"), 404);
                }

                await _unitOfWork.PParameters.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ParameterDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("Error_Delete"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
