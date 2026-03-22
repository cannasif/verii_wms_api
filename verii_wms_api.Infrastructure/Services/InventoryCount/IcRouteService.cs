using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class IcRouteService : IIcRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public IcRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<IcRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.IcRoutes.FindAsync(x => !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcRouteDto>>(entities);
                return ApiResponse<IEnumerable<IcRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<IcRouteDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<IcRouteDto>>.ErrorResult(
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

                var result = new PagedResponse<IcRouteDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<IcRouteDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<IcRouteDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<IcRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.IcRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteNotFound"), _localizationService.GetLocalizedString("IcRouteNotFound"), 404);
                }
                var dto = _mapper.Map<IcRouteDto>(entity);
                return ApiResponse<IcRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<IcRouteDto>>> GetByImportLineIdAsync(long importLineId)
        {
            try
            {
                var entities = await _unitOfWork.IcRoutes.FindAsync(x => x.ImportLineId == importLineId && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<IcRouteDto>>(entities);
                return ApiResponse<IEnumerable<IcRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("IcRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<IcRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<IcRouteDto>> CreateAsync(CreateIcRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<IcRoute>(createDto);
                entity.CreatedDate = DateTimeProvider.Now;
                entity.IsDeleted = false;
                await _unitOfWork.IcRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcRouteDto>(entity);
                return ApiResponse<IcRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IcRouteDto>> UpdateAsync(long id, UpdateIcRouteDto updateDto)
        {
            try
            {
                var entity = await _unitOfWork.IcRoutes.GetByIdAsync(id);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteNotFound"), _localizationService.GetLocalizedString("IcRouteNotFound"), 404);
                }
                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTimeProvider.Now;
                _unitOfWork.IcRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<IcRouteDto>(entity);
                return ApiResponse<IcRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("IcRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IcRouteDto>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.IcRoutes.GetByIdAsync(id);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcRouteNotFound"), _localizationService.GetLocalizedString("IcRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;

                // Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
                var remainingRoutesCount = await _unitOfWork.IcRoutes
                    .AsQueryable()
                    .CountAsync(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id);

                // Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
                var shouldDeleteImportLine = remainingRoutesCount == 0;

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // Route'u sil
                    await _unitOfWork.IcRoutes.SoftDelete(id);

                    // Eğer bu son route ise, ImportLine'ı da sil
                    if (shouldDeleteImportLine)
                    {
                        var importLine = await _unitOfWork.IcImportLines.GetByIdAsync(importLineId);
                        if (importLine != null && !importLine.IsDeleted)
                        {
                            await _unitOfWork.IcImportLines.SoftDelete(importLineId);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("IcRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("IcRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
