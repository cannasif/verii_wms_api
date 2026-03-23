using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class SitTerminalLineService : ISitTerminalLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public SitTerminalLineService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.SitTerminalLines.Query().ToListAsync();
                var dtos = _mapper.Map<IEnumerable<SitTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<SitTerminalLineDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<SitTerminalLineDto>>.ErrorResult(
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

                var result = new PagedResponse<SitTerminalLineDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<SitTerminalLineDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<SitTerminalLineDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<SitTerminalLineDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.SitTerminalLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineNotFound"), _localizationService.GetLocalizedString("SitTerminalLineNotFound"), 404);
                }
                var dto = _mapper.Map<SitTerminalLineDto>(entity);
                return ApiResponse<SitTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByHeaderIdAsync(long headerId)
        {
            try
            {
                var entities = await _unitOfWork.SitTerminalLines.Query().Where(x => x.HeaderId == headerId).ToListAsync();
                var dtos = _mapper.Map<IEnumerable<SitTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<SitTerminalLineDto>>> GetByUserIdAsync(long userId)
        {
            try
            {
                var entities = await _unitOfWork.SitTerminalLines.Query().Where(x => x.TerminalUserId == userId).ToListAsync();
                var dtos = _mapper.Map<IEnumerable<SitTerminalLineDto>>(entities);
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitTerminalLineRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SitTerminalLineDto>>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }


        public async Task<ApiResponse<SitTerminalLineDto>> CreateAsync(CreateSitTerminalLineDto createDto)
        {
            try
            {
                var entity = _mapper.Map<SitTerminalLine>(createDto);
                entity.CreatedDate = DateTimeProvider.Now;
                entity.IsDeleted = false;
                await _unitOfWork.SitTerminalLines.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitTerminalLineDto>(entity);
                return ApiResponse<SitTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitTerminalLineCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<SitTerminalLineDto>> UpdateAsync(long id, UpdateSitTerminalLineDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.SitTerminalLines.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineNotFound"), _localizationService.GetLocalizedString("SitTerminalLineNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;
                _unitOfWork.SitTerminalLines.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<SitTerminalLineDto>(entity);
                return ApiResponse<SitTerminalLineDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitTerminalLineUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<SitTerminalLineDto>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var exists = await _unitOfWork.SitTerminalLines.ExistsAsync(id);
                if (!exists)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineNotFound"), _localizationService.GetLocalizedString("SitTerminalLineNotFound"), 404);
                }
                await _unitOfWork.SitTerminalLines.SoftDelete(id);
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitTerminalLineDeletedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitTerminalLineErrorOccurred"), ex.Message ?? String.Empty, 500);
            }
        }
    }
}
