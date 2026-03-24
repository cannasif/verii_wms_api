using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using System.Security.Claims;

namespace WMS_WEBAPI.Services
{
    public class SitHeaderService : ISitHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErpService _erpService;
        private readonly INotificationService _notificationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SitHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHttpContextAccessor httpContextAccessor, IErpService erpService, INotificationService notificationService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
            _erpService = erpService;
            _notificationService = notificationService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }
        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
var entities = await _unitOfWork.SitHeaders.Query().Where(x => x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);

var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;

return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
var query = _unitOfWork.SitHeaders.Query().Where(x => x.BranchCode == branchCode);

query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SitHeaderDto>>(items);

var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data?.ToList() ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

var result = new PagedResponse<SitHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SitHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitHeaders.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
    return ApiResponse<SitHeaderDto>.ErrorResult(nf, nf, 404);
}
var dto = _mapper.Map<SitHeaderDto>(entity);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
if (!enrichedCustomer.Success)
{
    return ApiResponse<SitHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
if (!enrichedWarehouse.Success)
{
    return ApiResponse<SitHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByBranchCodeAsync(string branchCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SitHeaders.Query().Where(x => x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
var entities = await _unitOfWork.SitHeaders.Query().Where(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByCustomerCodeAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";
var entities = await _unitOfWork.SitHeaders.Query().Where(x => x.CustomerCode == customerCode && x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentTypeAsync(string documentType, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SitHeaders.Query().Where(x => x.DocumentType == documentType).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }
        

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetByDocumentNoAsync(string documentNo, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SitHeaders.Query().Where(x => x.ERPReferenceNumber == documentNo).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitHeaderDto>> CreateAsync(CreateSitHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (createDto == null)
{
    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
}
if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
{
    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
}
if (createDto.YearCode?.Length != 4)
{
    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
}
if (createDto.PlannedDate == default)
{
    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
}
var entity = _mapper.Map<SitHeader>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.SitHeaders.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SitHeaderDto>(entity);
return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderCreatedSuccessfully"));
        }

        public async Task<ApiResponse<SitHeaderDto>> UpdateAsync(long id, UpdateSitHeaderDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitHeaders.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null)
{
    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderNotFound"), _localizationService.GetLocalizedString("SitHeaderNotFound"), 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.SitHeaders.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SitHeaderDto>(entity);
return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var exists = await _unitOfWork.SitHeaders.ExistsAsync(id);
if (!exists)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderNotFound"), _localizationService.GetLocalizedString("SitHeaderNotFound"), 404);
}
var importLines = await _unitOfWork.SitImportLines.Query().Where(x => x.HeaderId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("SitHeaderImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}
await _unitOfWork.SitHeaders.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitHeaderDeletedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SitHeaders.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null)
{
    var notFound = _localizationService.GetLocalizedString("SitHeaderNotFound");
    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
}

// ============================================
// CHECK ERP APPROVAL REQUIREMENT
// ============================================
var sitParameter = await _unitOfWork.SitParameters
    .Query()
    .FirstOrDefaultAsync(requestCancellationToken);

// ============================================
// VALIDATE LINE SERIAL VS ROUTE QUANTITIES
// ============================================
// Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
// Normalize null values to false
bool skipValidation = (sitParameter?.AllowLessQuantityBasedOnOrder ?? false) 
    && (sitParameter?.AllowMoreQuantityBasedOnOrder ?? false);

// Normalize RequireAllOrderItemsCollected
bool requireAllOrderItemsCollected = sitParameter?.RequireAllOrderItemsCollected ?? false;

if (!skipValidation)
{
    var lines = await _unitOfWork.SitLines
        .Query()
        .Where(l => l.HeaderId == id)
        .ToListAsync(requestCancellationToken);

    foreach (var line in lines)
    {
        // Get total quantity of LineSerials for this Line
        var totalLineSerialQuantity = await _unitOfWork.SitLineSerials
            .Query()
            .Where(ls => ls.LineId == line.Id)
            .SumAsync(ls => ls.Quantity);

        // Get total quantity of Routes for ImportLines linked to this Line
        var totalRouteQuantity = await _unitOfWork.SitRoutes
            .Query()
            .Where(r => r.ImportLine.LineId == line.Id 
                && !r.ImportLine.IsDeleted)
            .SumAsync(r => r.Quantity);

        // ============================================
        // CHECK IF ALL ORDER ITEMS MUST BE COLLECTED
        // ============================================
        if (requireAllOrderItemsCollected)
        {
            // If RequireAllOrderItemsCollected is true, every line must have routes (totalRouteQuantity > 0)
            if (totalRouteQuantity <= 0.000001m)
            {
                var msg = _localizationService.GetLocalizedString("SitHeaderAllOrderItemsMustBeCollected", 
                    line.Id, 
                    line.StockCode ?? string.Empty, 
                    line.YapKod ?? string.Empty);
                return ApiResponse<bool>.ErrorResult(msg, 
                    $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): All order items must be collected. Route quantity is 0.", 
                    400);
            }
        }
        else
        {
            // If RequireAllOrderItemsCollected is false, skip validation for lines with no routes
            if (totalRouteQuantity <= 0.000001m)
            {
                continue; // Skip this line, no routes means it's optional
            }
        }

        // Determine validation logic based on parameters
        // Normalize null values to false
        bool allowLess = sitParameter?.AllowLessQuantityBasedOnOrder ?? false;
        bool allowMore = sitParameter?.AllowMoreQuantityBasedOnOrder ?? false;
        
        bool quantityMismatch = false;
        string localizedMessage = string.Empty;
        string exceptionMessage = string.Empty;

        if (!allowLess && !allowMore)
        {
            // Both false: Exact match required (==)
            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
            {
                quantityMismatch = true;
                localizedMessage = _localizationService.GetLocalizedString("SitHeaderQuantityExactMatchRequired", 
                    line.Id, 
                    line.StockCode ?? string.Empty, 
                    line.YapKod ?? string.Empty, 
                    totalLineSerialQuantity, 
                    totalRouteQuantity);
                exceptionMessage = $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): LineSerial total ({totalLineSerialQuantity}) must exactly match Route total ({totalRouteQuantity})";
            }
        }
        else if (allowLess && !allowMore)
        {
            // AllowLessQuantityBasedOnOrder: true, AllowMoreQuantityBasedOnOrder: false
            // Route <= LineSerial (Route can be less or equal to LineSerial)
            // Error if Route > LineSerial
            if (totalRouteQuantity > totalLineSerialQuantity + 0.000001m)
            {
                quantityMismatch = true;
                localizedMessage = _localizationService.GetLocalizedString("SitHeaderQuantityCannotBeGreater", 
                    line.Id, 
                    line.StockCode ?? string.Empty, 
                    line.YapKod ?? string.Empty, 
                    totalLineSerialQuantity, 
                    totalRouteQuantity);
                exceptionMessage = $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): Route total ({totalRouteQuantity}) cannot be greater than LineSerial total ({totalLineSerialQuantity})";
            }
        }
        else if (!allowLess && allowMore)
        {
            // AllowLessQuantityBasedOnOrder: false, AllowMoreQuantityBasedOnOrder: true
            // Route >= LineSerial (Route can be more or equal to LineSerial)
            // Error if Route < LineSerial
            if (totalRouteQuantity + 0.000001m < totalLineSerialQuantity)
            {
                quantityMismatch = true;
                localizedMessage = _localizationService.GetLocalizedString("SitHeaderQuantityCannotBeLess", 
                    line.Id, 
                    line.StockCode ?? string.Empty, 
                    line.YapKod ?? string.Empty, 
                    totalLineSerialQuantity, 
                    totalRouteQuantity);
                exceptionMessage = $"Line {line.Id} (StockCode: {line.StockCode}, YapKod: {line.YapKod ?? "N/A"}): Route total ({totalRouteQuantity}) cannot be less than LineSerial total ({totalLineSerialQuantity})";
            }
        }

        if (quantityMismatch)
        {
            return ApiResponse<bool>.ErrorResult(localizedMessage, exceptionMessage, 400);
        }
    }
}

// ============================================
// TRANSACTION: Start transaction for write operations
// ============================================
using var tx = await _unitOfWork.BeginTransactionAsync();
try
{
    entity.IsCompleted = true;
    entity.CompletionDate = DateTimeProvider.Now;
    
    // Set IsPendingApproval based on parameter requirement
    entity.IsPendingApproval = sitParameter != null && sitParameter.RequireApprovalBeforeErp;
    _unitOfWork.SitHeaders.Update(entity);

    // Update package status to Shipped
    var package = _unitOfWork.PHeaders.Query(tracking: true)
        .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.SIT)
        .FirstOrDefault();
    if (package != null)
    {
        package.Status = PHeaderStatus.Shipped;
        _unitOfWork.PHeaders.Update(package);
    }

    // Create notification for the user who created the order
    Notification? notification = null;
    if (entity.CreatedBy.HasValue)
    {
        var orderNumber = entity.Id.ToString();
        notification = new Notification
        {
            Title = _localizationService.GetLocalizedString("SitDoneNotificationTitle", orderNumber),
            Message = _localizationService.GetLocalizedString("SitDoneNotificationMessage", orderNumber),
            TitleKey = "SitDoneNotificationTitle",
            MessageKey = "SitDoneNotificationMessage",
            Channel = NotificationChannel.Web,
            Severity = NotificationSeverity.Info,
            RecipientUserId = entity.CreatedBy.Value,
            RelatedEntityType = NotificationEntityType.SITDone,
            RelatedEntityId = entity.Id,
            DeliveredAt = DateTimeProvider.Now
        };

        await _unitOfWork.Notifications.AddAsync(notification);
    }

    // Single SaveChanges for both header update and notification
    await _unitOfWork.SaveChangesAsync(requestCancellationToken);
    await _unitOfWork.CommitTransactionAsync();

    // Publish SignalR notification after transaction is committed
    if (notification != null)
    {
        await _notificationService.PublishSignalRNotificationsAsync(new[] { notification });
    }

    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SitHeaderCompletedSuccessfully"));
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
        }

        public async Task<ApiResponse<IEnumerable<SitHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _httpContextAccessor.HttpContext?.Items["BranchCode"] as string ?? "0";

// Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
// SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
// Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
var query = _unitOfWork.SitHeaders
    .Query()
    .Where(h => !h.IsCompleted 
        && h.BranchCode == branchCode
        && _unitOfWork.SitTerminalLines
            .Query(false, false)
            .Any(t => t.HeaderId == h.Id 
                && t.TerminalUserId == userId));

var entities = await query.ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SitHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SitHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SitHeaderAssignedOrdersRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lines = await _unitOfWork.SitLines
    .Query()
    .Where(x => x.HeaderId == headerId)
    .ToListAsync(requestCancellationToken);

var lineIds = lines.Select(l => l.Id).ToList();

IEnumerable<SitLineSerial> lineSerials = Array.Empty<SitLineSerial>();
if (lineIds.Count > 0)
{
    lineSerials = await _unitOfWork.SitLineSerials
        .Query()
        .Where(x => lineIds.Contains(x.LineId))
        .ToListAsync(requestCancellationToken);
}

var importLines = await _unitOfWork.SitImportLines
    .Query()
    .Where(x => x.HeaderId == headerId)
    .ToListAsync(requestCancellationToken);

var importLineIds = importLines.Select(il => il.Id).ToList();

IEnumerable<SitRoute> routes = Array.Empty<SitRoute>();
if (importLineIds.Count > 0)
{
    routes = await _unitOfWork.SitRoutes
        .Query()
        .Where(x => importLineIds.Contains(x.ImportLineId))
        .ToListAsync(requestCancellationToken);
}

var lineDtos = _mapper.Map<IEnumerable<SitLineDto>>(lines);
if (lineDtos.Any())
{
    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
    if (enrichedLines.Success)
    {
        lineDtos = enrichedLines.Data ?? lineDtos;
    }
}

var importLineDtos = _mapper.Map<IEnumerable<SitImportLineDto>>(importLines);
if (importLineDtos.Any())
{
    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
    if (enrichedImportLines.Success)
    {
        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
    }
}

var dto = new SitAssignedOrderLinesDto
{
    Lines = lineDtos,
    LineSerials = _mapper.Map<IEnumerable<SitLineSerialDto>>(lineSerials),
    ImportLines = importLineDtos,
    Routes = _mapper.Map<IEnumerable<SitRouteDto>>(routes)
};

return ApiResponse<SitAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderAssignedOrderLinesRetrievedSuccessfully"));
        }

        

        public async Task<ApiResponse<PagedResponse<SitHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.SitHeaders.Query()
    .Where(x => x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SitHeaderDto>>(items);

var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data?.ToList() ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<PagedResponse<SitHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

var result = new PagedResponse<SitHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SitHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SitHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SitHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
// Tracking ile yükle (navigation property'ler yüklenmeyecek)
var entity = await _unitOfWork.SitHeaders
    .Query(tracking: true)
    .Where(e => e.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
    
if (entity == null)
{
    var nf = _localizationService.GetLocalizedString("SitHeaderNotFound");
    return ApiResponse<SitHeaderDto>.ErrorResult(nf, nf, 404);
}

if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
{
    var msg = _localizationService.GetLocalizedString("SitHeaderApprovalUpdateError");
    return ApiResponse<SitHeaderDto>.ErrorResult(msg, msg, 400);
}

var httpUser = _httpContextAccessor.HttpContext?.User;
long? approvedByUserId = null;
var claimVal = httpUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
if (long.TryParse(claimVal, out var uid))
{
    approvedByUserId = uid;
}

entity.ApprovalStatus = approved;
entity.ApprovedByUserId = approvedByUserId;
entity.ApprovalDate = DateTime.Now;
entity.IsPendingApproval = false;

_unitOfWork.SitHeaders.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<SitHeaderDto>(entity);
return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderApprovalUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<SitHeaderDto>> GenerateOrderAsync(GenerateSubcontractingIssueOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
using (var tx = await _unitOfWork.BeginTransactionAsync())
{
    try
    {
        var header = _mapper.Map<SitHeader>(request.Header) ?? new SitHeader();
        await _unitOfWork.SitHeaders.AddAsync(header);
        await _unitOfWork.SaveChangesAsync(requestCancellationToken);

        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        var lineGuidToId = new Dictionary<Guid, long>();

        if (request.Lines != null && request.Lines.Count > 0)
        {
            var lines = new List<SitLine>(request.Lines.Count);
            foreach (var l in request.Lines)
            {
                var line = _mapper.Map<SitLine>(l);
                line.HeaderId = header.Id;
                lines.Add(line);
            }
            await _unitOfWork.SitLines.AddRangeAsync(lines);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);

            for (int i = 0; i < request.Lines.Count; i++)
            {
                var key = request.Lines[i].ClientKey;
                var guid = request.Lines[i].ClientGuid;
                var id = lines[i].Id;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    lineKeyToId[key!] = id;
                }
                if (guid.HasValue)
                {
                    lineGuidToId[guid.Value] = id;
                }
            }
        }

        if (request.LineSerials != null && request.LineSerials.Count > 0)
        {
            var serials = new List<SitLineSerial>(request.LineSerials.Count);
            foreach (var s in request.LineSerials)
            {
                long lineId = 0;
                if (s.LineGroupGuid.HasValue)
                {
                    var lg = s.LineGroupGuid.Value;
                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), _localizationService.GetLocalizedString("SitHeaderErrorOccurred"), 400);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                {
                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), _localizationService.GetLocalizedString("SitHeaderErrorOccurred"), 400);
                    }
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<SitHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SitHeaderErrorOccurred"), _localizationService.GetLocalizedString("SitHeaderErrorOccurred"), 400);
                }

                var serial = _mapper.Map<SitLineSerial>(s);
                serial.LineId = lineId;
                serials.Add(serial);
            }
            await _unitOfWork.SitLineSerials.AddRangeAsync(serials);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);
        }

        List<Notification> createdNotifications = new List<Notification>();
        
        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
        {
            var tlines = new List<SitTerminalLine>(request.TerminalLines.Count);
            foreach (var t in request.TerminalLines)
            {
                var tline = _mapper.Map<SitTerminalLine>(t);
                tline.HeaderId = header.Id;
                tlines.Add(tline);
            }
            await _unitOfWork.SitTerminalLines.AddRangeAsync(tlines);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);

            // Create and add notifications for each terminal line
            var orderNumber = header.Id.ToString();
            createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                tlines,
                orderNumber,
                NotificationEntityType.SITHeader,
                "SIT_HEADER",
                "SitHeaderNotificationTitle",
                "SitHeaderNotificationMessage"
            );
            
            // Save notifications to database (they will be committed with transaction)
            if (createdNotifications.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(requestCancellationToken);
            }
        }

        await _unitOfWork.CommitTransactionAsync();

        // Publish SignalR notifications after transaction is committed
        if (createdNotifications.Count > 0)
        {
            await _notificationService.PublishSignalRNotificationsForCreatedNotificationsAsync(createdNotifications);
        }

        var dto = _mapper.Map<SitHeaderDto>(header);
        return ApiResponse<SitHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SitHeaderCreatedSuccessfully"));
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}
        }

        public async Task<ApiResponse<int>> BulkCreateSubcontractingIssueTransferAsync(BulkCreateSitRequestDto request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
using (var tx = await _unitOfWork.BeginTransactionAsync())
{
    try
    {
        // ============================================
        // 1. VALIDATION
        // ============================================
        if (request == null || request.Header == null)
        {
            return ApiResponse<int>.ErrorResult(
                _localizationService.GetLocalizedString("InvalidModelState"),
                _localizationService.GetLocalizedString("RequestOrHeaderMissing"),
                400);
        }

        // ============================================
        // 1.1. CHECK ERP APPROVAL REQUIREMENT
        // ============================================
        var sitParameter = await _unitOfWork.SitParameters
            .Query()
            .FirstOrDefaultAsync(requestCancellationToken);

        // ============================================
        // 2. CREATE HEADER
        // ============================================
        var header = _mapper.Map<SitHeader>(request.Header);
        
        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
        header.IsPendingApproval = sitParameter != null && sitParameter.RequireApprovalBeforeErp;

        await _unitOfWork.SitHeaders.AddAsync(header);
        await _unitOfWork.SaveChangesAsync(requestCancellationToken);

        if (header.Id <= 0)
        {
            await tx.RollbackAsync();
            return ApiResponse<int>.ErrorResult(
                _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                _localizationService.GetLocalizedString("HeaderInsertFailed"),
                500);
        }

        // ============================================
        // 3. CREATE LINES & BUILD KEY-TO-ID MAPPING
        // ============================================
        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        var lineGuidToId = new Dictionary<Guid, long>();
        if (request.Lines != null && request.Lines.Count > 0)
        {
            var lines = new List<SitLine>(request.Lines.Count);
            foreach (var lineDto in request.Lines)
            {
                var line = _mapper.Map<SitLine>(lineDto) ?? new SitLine();
                line.HeaderId = header.Id;
                lines.Add(line);
            }

            await _unitOfWork.SitLines.AddRangeAsync(lines);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);

            // Build ClientKey -> Id and ClientGuid -> Id mappings
            for (int i = 0; i < request.Lines.Count; i++)
            {
                var key = request.Lines[i].ClientKey;
                var guid = request.Lines[i].ClientGuid;
                var id = lines[i].Id;

                if (!string.IsNullOrWhiteSpace(key))
                {
                    lineKeyToId[key] = id;
                }
                if (guid.HasValue)
                {
                    lineGuidToId[guid.Value] = id;
                }
            }
        }

        // ============================================
        // 4. CREATE LINE SERIALS
        // ============================================
        if (request.LineSerials != null && request.LineSerials.Count > 0)
        {
            var serials = new List<SitLineSerial>(request.LineSerials.Count);
            foreach (var serialDto in request.LineSerials)
            {
                // LineSerial requires LineId - find by LineGroupGuid first, then LineClientKey
                long lineId;
                if (serialDto.LineGroupGuid.HasValue)
                {
                    if (!lineGuidToId.TryGetValue(serialDto.LineGroupGuid.Value, out var foundLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("LineGroupGuidNotFound"),
                            400);
                    }
                    lineId = foundLineId;
                }
                else if (!string.IsNullOrWhiteSpace(serialDto.LineClientKey))
                {
                    if (!lineKeyToId.TryGetValue(serialDto.LineClientKey, out var foundLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("LineClientKeyNotFound"),
                            400);
                    }
                    lineId = foundLineId;
                }
                else
                {
                    await tx.RollbackAsync();
                    return ApiResponse<int>.ErrorResult(
                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                        _localizationService.GetLocalizedString("LineReferenceMissing"),
                        400);
                }

                var serial = _mapper.Map<SitLineSerial>(serialDto) ?? new SitLineSerial();
                serial.LineId = lineId;
                serials.Add(serial);
            }

            await _unitOfWork.SitLineSerials.AddRangeAsync(serials);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);
        }

        // ============================================
        // 5. CREATE IMPORT LINES & BUILD KEY-TO-ID MAPPING
        // ============================================
        var importLineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        var importLineGuidToId = new Dictionary<Guid, long>();
        var routeKeyToImportLineId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        var routeGuidToImportLineId = new Dictionary<Guid, long>();

        if (request.ImportLines != null && request.ImportLines.Count > 0)
        {
            var importLines = new List<SitImportLine>(request.ImportLines.Count);
            foreach (var importDto in request.ImportLines)
            {
                // ClientKey is required for correlation
                if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                {
                    await tx.RollbackAsync();
                    return ApiResponse<int>.ErrorResult(
                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                        _localizationService.GetLocalizedString("ImportLineClientKeyMissing"),
                        400);
                }

                // LineClientKey/LineGroupGuid is optional - if provided, validate and link to Line
                long? lineId = null;
                if (importDto.LineGroupGuid.HasValue)
                {
                    if (!lineGuidToId.TryGetValue(importDto.LineGroupGuid.Value, out var foundLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("LineGroupGuidNotFound"),
                            400);
                    }
                    lineId = foundLineId;
                }
                else if (!string.IsNullOrWhiteSpace(importDto.LineClientKey))
                {
                    if (!lineKeyToId.TryGetValue(importDto.LineClientKey, out var foundLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("LineClientKeyNotFound"),
                            400);
                    }
                    lineId = foundLineId;
                }

                var importLine = _mapper.Map<SitImportLine>(importDto) ?? new SitImportLine();
                importLine.HeaderId = header.Id;
                importLine.LineId = lineId;
                importLines.Add(importLine);
            }

            await _unitOfWork.SitImportLines.AddRangeAsync(importLines);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);

            // Build ClientKey -> Id, ClientGroupGuid -> Id, RouteClientKey -> Id, RouteGroupGuid -> Id mappings
            for (int i = 0; i < request.ImportLines.Count; i++)
            {
                var id = importLines[i].Id;
                var clientKey = request.ImportLines[i].ClientKey;
                var clientGuid = request.ImportLines[i].ClientGroupGuid;
                var routeKey = request.ImportLines[i].RouteClientKey;
                var routeGuid = request.ImportLines[i].RouteGroupGuid;

                if (!string.IsNullOrWhiteSpace(clientKey))
                {
                    importLineKeyToId[clientKey] = id;
                }
                if (clientGuid.HasValue)
                {
                    importLineGuidToId[clientGuid.Value] = id;
                }
                if (!string.IsNullOrWhiteSpace(routeKey))
                {
                    routeKeyToImportLineId[routeKey] = id;
                }
                if (routeGuid.HasValue)
                {
                    routeGuidToImportLineId[routeGuid.Value] = id;
                }
            }
        }

        // ============================================
        // 6. CREATE ROUTES
        // ============================================
        if (request.Routes != null && request.Routes.Count > 0)
        {
            var routes = new List<SitRoute>(request.Routes.Count);
            foreach (var routeDto in request.Routes)
            {
                // Find ImportLineId by priority: ImportLineGroupGuid > ImportLineClientKey > RouteGroupGuid > RouteClientKey
                long? importLineId = null;

                if (routeDto.ImportLineGroupGuid.HasValue)
                {
                    if (!importLineGuidToId.TryGetValue(routeDto.ImportLineGroupGuid.Value, out var foundImportLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("ImportLineGroupGuidNotFound"),
                            400);
                    }
                    importLineId = foundImportLineId;
                }
                else if (!string.IsNullOrWhiteSpace(routeDto.ImportLineClientKey))
                {
                    if (!importLineKeyToId.TryGetValue(routeDto.ImportLineClientKey, out var foundImportLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("ImportLineClientKeyNotFound"),
                            400);
                    }
                    importLineId = foundImportLineId;
                }
                else if (routeDto.ClientGroupGuid.HasValue)
                {
                    if (!routeGuidToImportLineId.TryGetValue(routeDto.ClientGroupGuid.Value, out var foundImportLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("RouteGroupGuidNotFound"),
                            400);
                    }
                    importLineId = foundImportLineId;
                }
                else if (!string.IsNullOrWhiteSpace(routeDto.ClientKey))
                {
                    if (!routeKeyToImportLineId.TryGetValue(routeDto.ClientKey, out var foundImportLineId))
                    {
                        await tx.RollbackAsync();
                        return ApiResponse<int>.ErrorResult(
                            _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                            _localizationService.GetLocalizedString("RouteClientKeyNotFound"),
                            400);
                    }
                    importLineId = foundImportLineId;
                }

                if (!importLineId.HasValue)
                {
                    await tx.RollbackAsync();
                    return ApiResponse<int>.ErrorResult(
                        _localizationService.GetLocalizedString("SitHeaderErrorOccurred"),
                        _localizationService.GetLocalizedString("ImportLineReferenceMissing"),
                        400);
                }

                var route = _mapper.Map<SitRoute>(routeDto) ?? new SitRoute();
                route.ImportLineId = importLineId.Value;
                routes.Add(route);
            }

            await _unitOfWork.SitRoutes.AddRangeAsync(routes);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);
        }

        // ============================================
        // 7. COMMIT TRANSACTION
        // ============================================
        await tx.CommitAsync();
        return ApiResponse<int>.SuccessResult(
            1,
            _localizationService.GetLocalizedString("OperationSuccessful"));
    }
    catch
    {
        await tx.RollbackAsync();
        throw;
    }
}
        }
    }
}
