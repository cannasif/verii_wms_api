using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrRouteService : IGrRouteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GrRouteService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IRequestCancellationAccessor requestCancellationAccessor)
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


        public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var items = await _unitOfWork.GrRoutes.Query().ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<GrRouteDto>>(items);
return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<GrRouteDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.GrRoutes.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<GrRouteDto>>(items);
var result = new PagedResponse<GrRouteDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<GrRouteDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrRouteDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var item = await _unitOfWork.GrRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (item == null || item.IsDeleted)
{
    var nf = _localizationService.GetLocalizedString("GrRouteNotFound");
    return ApiResponse<GrRouteDto>.ErrorResult(nf, nf, 404);
}
var dto = _mapper.Map<GrRouteDto>(item);
return ApiResponse<GrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByImportLineIdAsync(long importLineId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var items = await _unitOfWork.GrRoutes.Query().Where(x => x.ImportLineId == importLineId).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<GrRouteDto>>(items);
return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrRouteDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.GrRoutes.Query().Where(x => x.ImportLine.HeaderId == headerId && x.ImportLine.IsDeleted == false);
var items = await query.ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<GrRouteDto>>(items);
return ApiResponse<IEnumerable<GrRouteDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("GrRouteRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrRouteDto>> CreateAsync(CreateGrRouteDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<GrRoute>(createDto);
await _unitOfWork.GrRoutes.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<GrRouteDto>(entity);
return ApiResponse<GrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrRouteCreatedSuccessfully"));
        }

        public async Task<ApiResponse<GrRouteDto>> UpdateAsync(long id, UpdateGrRouteDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.GrRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    var nf = _localizationService.GetLocalizedString("GrRouteNotFound");
    return ApiResponse<GrRouteDto>.ErrorResult(nf, nf, 404);
}
_mapper.Map(updateDto, entity);
_unitOfWork.GrRoutes.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<GrRouteDto>(entity);
return ApiResponse<GrRouteDto>.SuccessResult(dto, _localizationService.GetLocalizedString("GrRouteUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var route = await _unitOfWork.GrRoutes.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (route == null || route.IsDeleted)
{
    var nf = _localizationService.GetLocalizedString("GrRouteNotFound");
    return ApiResponse<bool>.ErrorResult(nf, nf, 404);
}

var importLineId = route.ImportLineId;

// Bu ImportLine'a bağlı, silinmemiş ve bu route dışında başka route var mı kontrol et
var remainingRoutesCount = await _unitOfWork.GrRoutes.Query()
    .Where(r => !r.IsDeleted && r.ImportLineId == importLineId && r.Id != id)
            .CountAsync(requestCancellationToken);

// Eğer başka route yoksa (count == 0), bu son route demektir, ImportLine'ı da silmeliyiz
var shouldDeleteImportLine = remainingRoutesCount == 0;

using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    // Route'u sil
    await _unitOfWork.GrRoutes.SoftDelete(id);

    // Eğer bu son route ise, ImportLine'ı da sil
    if (shouldDeleteImportLine)
    {
        var importLine = await _unitOfWork.GrImportLines.GetByIdAsync(importLineId);
        if (importLine != null && !importLine.IsDeleted)
        {
            await _unitOfWork.GrImportLines.SoftDelete(importLineId);
        }
    }

    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await tx.CommitAsync();
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrRouteDeletedSuccessfully"));
}
catch
{
    await tx.RollbackAsync();
    throw;
}
        }
    }
}
