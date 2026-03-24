using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;

namespace WMS_WEBAPI.Services
{
    public class GrImportDocumentService : IGrImportDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public GrImportDocumentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IRequestCancellationAccessor requestCancellationAccessor
        )
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


        public async Task<ApiResponse<PagedResponse<GrImportDocumentDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.GrImportDocuments.Query();
query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<GrImportDocumentDto>>(items);

var result = new PagedResponse<GrImportDocumentDto>(dtos, totalCount, request.PageNumber, request.PageSize);

return ApiResponse<PagedResponse<GrImportDocumentDto>>.SuccessResult(result, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var documents = await _unitOfWork.GrImportDocuments.GetAllAsync();
var documentDtos = _mapper.Map<IEnumerable<GrImportDocumentDto>>(documents);

return ApiResponse<IEnumerable<GrImportDocumentDto>>.SuccessResult(documentDtos, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrImportDocumentDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var document = await _unitOfWork.GrImportDocuments.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (document == null)
{
    return ApiResponse<GrImportDocumentDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrImportDocumentNotFound"),
        _localizationService.GetLocalizedString("RecordNotFound"),
        404,
        _localizationService.GetLocalizedString("GrImportDocumentNotFound"));
}

var documentDto = _mapper.Map<GrImportDocumentDto>(document);
return ApiResponse<GrImportDocumentDto>.SuccessResult(documentDto, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<GrImportDocumentDto>>> GetByHeaderIdAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var documents = await _unitOfWork.GrImportDocuments.FindAsync(d => d.HeaderId == headerId);
var documentDtos = _mapper.Map<IEnumerable<GrImportDocumentDto>>(documents);

return ApiResponse<IEnumerable<GrImportDocumentDto>>.SuccessResult(documentDtos, _localizationService.GetLocalizedString("GrImportDocumentRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<GrImportDocumentDto>> CreateAsync(CreateGrImportDocumentDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
// HeaderId'nin geçerli olup olmadığını kontrol et
var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)createDto.HeaderId);
if (!headerExists)
{
    return ApiResponse<GrImportDocumentDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrImportDocumentInvalidHeaderId"),
        _localizationService.GetLocalizedString("HeaderNotFound"),
        404,
        _localizationService.GetLocalizedString("HeaderNotFound"));
}

var document = _mapper.Map<GrImportDocument>(createDto);
var createdDocument = await _unitOfWork.GrImportDocuments.AddAsync(document);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var documentDto = _mapper.Map<GrImportDocumentDto>(createdDocument);
return ApiResponse<GrImportDocumentDto>.SuccessResult(documentDto, _localizationService.GetLocalizedString("GrImportDocumentCreatedSuccessfully"));
        }

        public async Task<ApiResponse<GrImportDocumentDto>> UpdateAsync(long id, UpdateGrImportDocumentDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var document = await _unitOfWork.GrImportDocuments.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (document == null)
{
    return ApiResponse<GrImportDocumentDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrImportDocumentNotFound"),
        _localizationService.GetLocalizedString("RecordNotFound"),
        404,
        _localizationService.GetLocalizedString("GrImportDocumentNotFound"));
}

// HeaderId'nin geçerli olup olmadığını kontrol et
var headerExists = await _unitOfWork.GrHeaders.ExistsAsync((int)updateDto.HeaderId);
if (!headerExists)
{
    return ApiResponse<GrImportDocumentDto>.ErrorResult(
        _localizationService.GetLocalizedString("GrImportDocumentInvalidHeaderId"),
        _localizationService.GetLocalizedString("HeaderNotFound"),
        404,
        _localizationService.GetLocalizedString("HeaderNotFound"));
}

_mapper.Map(updateDto, document);
_unitOfWork.GrImportDocuments.Update(document);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var documentDto = _mapper.Map<GrImportDocumentDto>(document);
return ApiResponse<GrImportDocumentDto>.SuccessResult(documentDto, _localizationService.GetLocalizedString("GrImportDocumentUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var document = await _unitOfWork.GrImportDocuments.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (document == null)
{
    return ApiResponse<bool>.ErrorResult(
        _localizationService.GetLocalizedString("GrImportDocumentNotFound"),
        _localizationService.GetLocalizedString("RecordNotFound"),
        404,
        _localizationService.GetLocalizedString("GrImportDocumentNotFound"));
}

await _unitOfWork.GrImportDocuments.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("GrImportDocumentDeletedSuccessfully"));
        }

        
    }
}
