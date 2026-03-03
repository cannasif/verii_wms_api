using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class IcTerminalLineService : IIcTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public IcTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<IcTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.IcTerminalLines.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<IcTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<IcTerminalLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<IcTerminalLineDto>>.ErrorResult(
                        allResult.Message,
                        allResult.ExceptionMessage,
                        allResult.StatusCode);
                }

                var query = allResult.Data.AsQueryable();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = query.Count();
                var items = query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToList();

                var result = new PagedResponse<IcTerminalLineDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<IcTerminalLineDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<IcTerminalLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<IcTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.IcTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineNotFound"), _localizationService.GetLocalizedString("IcTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<IcTerminalLineDto>(entity);
                return ApiResponse<IcTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<IcTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.IcTerminalLines.FindAsync(x => x.HeaderId == headerId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<IcTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IcTerminalLineDto>> CreateAsync(CreateIcTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<IcTerminalLine>(createDto);
                entity.CreatedDate = DateTime.UtcNow;
                entity.IsDeleted = false;
                await _unitOfWork.IcTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcTerminalLineDto>(entity);
                return ApiResponse<IcTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IcTerminalLineDto>> UpdateAsync(long id, UpdateIcTerminalLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.IcTerminalLines.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineNotFound"), _localizationService.GetLocalizedString("IcTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.UtcNow;
                _unitOfWork.IcTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcTerminalLineDto>(entity);
                return ApiResponse<IcTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.IcTerminalLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineNotFound"), _localizationService.GetLocalizedString("IcTerminalLineNotFound"), 404);
                }
                await _unitOfWork.IcTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcTerminalLineErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}