using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class ShRouteService : IShRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;

        public ShRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<ShRouteDto>>> GetPagedAsync(PagedRequest request)
        {
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var query = _unitOfWork.ShRoutes.Query();
                query = query.ApplyFilters(request.Filters, request.FilterLogic);
                bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
                query = query.ApplySorting(request.SortBy ?? "Id", desc);

                var totalCount = await query.CountAsync();
                var entities = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<List<ShRouteDto>>(entities);
                var result = new PagedResponse<ShRouteDto>(dtos, totalCount, request.PageNumber, request.PageSize);

                return ApiResponse<PagedResponse<ShRouteDto>>.SuccessResult(
                    result,
                    _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<ShRouteDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("ShRouteErrorOccurred"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<ShRouteDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _unitOfWork.ShRoutes.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (entity == null) { var nf = _localizationService.GetLocalizedString("ShRouteNotFound"); return ApiResponse<ShRouteDto>.ErrorResult(nf, nf, 404); }
                var dto = _mapper.Map<ShRouteDto>(entity);
                return ApiResponse<ShRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShRouteDto>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByStockCodeAsync(string stockCode)
        {
            try
            {
                var query = _unitOfWork.ShRoutes.Query().Where(r => ((r.ImportLine.StockCode ?? "").Trim() == (stockCode ?? "").Trim()) && !r.IsDeleted);
                var entities = await query.ToListAsync();
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySerialNoAsync(string serialNo)
        {
            try
            {
                var sn = (serialNo ?? "").Trim();
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => (((x.SerialNo ?? "").Trim() == sn) || ((x.SerialNo2 ?? "").Trim() == sn) || ((x.SerialNo3 ?? "").Trim() == sn) || ((x.SerialNo4 ?? "").Trim() == sn)) && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetBySourceWarehouseAsync(int sourceWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => x.SourceWarehouse == sourceWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<ShRouteDto>>> GetByTargetWarehouseAsync(int targetWarehouse)
        {
            try
            {
                var entities = await _unitOfWork.ShRoutes.FindAsync(x => x.TargetWarehouse == targetWarehouse && !x.IsDeleted);
                var dtos = _mapper.Map<IEnumerable<ShRouteDto>>(entities);
                return ApiResponse<IEnumerable<ShRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("ShRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ShRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }


        public async Task<ApiResponse<ShRouteDto>> CreateAsync(CreateShRouteDto createDto)
        {
            try
            {
                var entity = _mapper.Map<ShRoute>(createDto);
                var created = await _unitOfWork.ShRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShRouteDto>(created);
                return ApiResponse<ShRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShRouteDto>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<ShRouteDto>> UpdateAsync(long id, UpdateShRouteDto updateDto)
        {
            try
            {
                var existing = await _unitOfWork.ShRoutes.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (existing == null) { var nf = _localizationService.GetLocalizedString("ShRouteNotFound"); return ApiResponse<ShRouteDto>.ErrorResult(nf, nf, 404); }
                var entity = _mapper.Map(updateDto, existing);
                _unitOfWork.ShRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                var dto = _mapper.Map<ShRouteDto>(entity);
                return ApiResponse<ShRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("ShRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<ShRouteDto>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id)
        {
            try
            {
                var route = await _unitOfWork.ShRoutes.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (route == null || route.IsDeleted)
                {
                    var nf = _localizationService.GetLocalizedString("ShRouteNotFound");
                    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
                }

                var importLineId = route.ImportLineId;

                // Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
                var remainingRoutesCount = await _unitOfWork.ShRoutes.Query()
                    .Where(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id)
                            .CountAsync();

                // Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
                var shouldDeleteImportLine = remainingRoutesCount == 0;

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {
                    // Route'u sil
                    await _unitOfWork.ShRoutes.SoftDelete(id);

                    // Eğer bu son route ise, ImportLine'ı da sil
                    if (shouldDeleteImportLine)
                    {
                        var importLine = await _unitOfWork.ShImportLines.GetByIdAsync(importLineId);
                        if (importLine != null && !importLine.IsDeleted)
                        {
                            await _unitOfWork.ShImportLines.SoftDelete(importLineId);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    await tx.CommitAsync();
                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("ShRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("ShRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
    }
}
