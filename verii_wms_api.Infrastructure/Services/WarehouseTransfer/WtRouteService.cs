using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Linq;

namespace WMS_WEBAPI.Services
{
    public class WtRouteService : IWtRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public WtRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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
        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.WtRoutes.Query().ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteRetrievalError"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<PagedResponse<WtRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                request ??= new PagedRequest();
                if (request.PageNumber < 1) request.PageNumber = 1;
                if (request.PageSize < 1) request.PageSize = 20;

                var allResult = await GetAllAsync();
                if (!allResult.Success || allResult.Data == null)
                {
                    return ApiResponse<PagedResponse<WtRouteDto>>.ErrorResult(
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

                var result = new PagedResponse<WtRouteDto>(items, totalCount, request.PageNumber, request.PageSize);
                return ApiResponse<PagedResponse<WtRouteDto>>.SuccessResult(result, allResult.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<PagedResponse<WtRouteDto>>.ErrorResult(
                    _localizationService.GetLocalizedString("Error_GetAll"),
                    ex.Message ?? string.Empty,
                    500);
            }
        }


        public async Task<ApiResponse<WtRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = await _unitOfWork.WtRoutes.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);

                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                var dto = _mapper.Map<WtRouteDto>(entity);
                return ApiResponse<WtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<IEnumerable<WtRouteDto>>> GetBySerialNoAsync(string serialNo, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entities = await _unitOfWork.WtRoutes.Query().Where(x => x.SerialNo == serialNo).ToListAsync(requestCancellationToken);
                var dtos = _mapper.Map<IEnumerable<WtRouteDto>>(entities);
                return ApiResponse<IEnumerable<WtRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("WtRouteRetrievedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WtRouteDto>>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtRouteDto>> CreateAsync(CreateWtRouteDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = _mapper.Map<WtRoute>(createDto);
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = false;

                await _unitOfWork.WtRoutes.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                var dto = _mapper.Map<WtRouteDto>(entity);
                return ApiResponse<WtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtRouteCreatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<WtRouteDto>> UpdateAsync(long id, UpdateWtRouteDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var entity = await _unitOfWork.WtRoutes.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (entity == null || entity.IsDeleted)
                {
                    return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                _mapper.Map(updateDto, entity);
                entity.UpdatedDate = DateTime.Now;

                _unitOfWork.WtRoutes.Update(entity);
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);

                var dto = _mapper.Map<WtRouteDto>(entity);
                return ApiResponse<WtRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("WtRouteUpdatedSuccessfully"));
            }
            catch (Exception ex)
            {
                return ApiResponse<WtRouteDto>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            try
            {
                var route = await _unitOfWork.WtRoutes.Query()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync(requestCancellationToken);
                if (route == null || route.IsDeleted)
                {
                    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtRouteNotFound"), _localizationService.GetLocalizedString("WtRouteNotFound"), 404);
                }

                var importLineId = route.ImportLineId;


                var headerId = await _unitOfWork.WtImportLines.Query()
                    .Where(il => il.Id == importLineId && !il.IsDeleted)
                    .Select(il => il.HeaderId)
                    .FirstOrDefaultAsync(requestCancellationToken);

                // Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
                var remainingRoutesCount = await _unitOfWork.WtRoutes.Query()
                    .Where(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id)
                            .CountAsync(requestCancellationToken);

                    var packageInfoList = await (
                        from pl in _unitOfWork.PLines.Query()
                        join p in _unitOfWork.PPackages.Query() on pl.PackageId equals p.Id
                        join ph in _unitOfWork.PHeaders.Query() on p.PackingHeaderId equals ph.Id
                        where !pl.IsDeleted
                              && !p.IsDeleted
                              && !ph.IsDeleted
                              && pl.SourceRouteId.HasValue
                              && pl.SourceRouteId == id
                              && ph.SourceHeaderId == headerId
                              && ph.SourceType == "WT"
                        select new
                        {
                            PackageLineId = pl.Id
                        }
                    ).ToListAsync(requestCancellationToken);

                // Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
                var shouldDeleteImportLine = remainingRoutesCount == 0;

                using var tx = await _unitOfWork.BeginTransactionAsync();
                try
                {

                    // Package'ları sil
                    var packageLineId = packageInfoList.FirstOrDefault()?.PackageLineId ?? 0;
                    await _unitOfWork.PLines.SoftDelete(packageLineId);

                    // Route'u sil
                    await _unitOfWork.WtRoutes.SoftDelete(id);

                    // Eğer bu son route ise, ImportLine'ı da sil
                    if (shouldDeleteImportLine)
                    {
                        var importLine = await _unitOfWork.WtImportLines.GetByIdAsync(importLineId);
                        if (importLine != null && !importLine.IsDeleted)
                        {
                            await _unitOfWork.WtImportLines.SoftDelete(importLineId);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
                    await tx.CommitAsync();

                    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("WtRouteDeletedSuccessfully"));
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("WtRouteErrorOccurred"), ex.Message ?? string.Empty, 500);
            }
        }
   
    }
}
