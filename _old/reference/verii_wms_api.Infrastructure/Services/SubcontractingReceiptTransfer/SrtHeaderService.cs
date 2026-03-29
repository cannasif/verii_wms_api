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
    public class SrtHeaderService : ISrtHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IErpService _erpService;
        private readonly INotificationService _notificationService;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public SrtHeaderService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, ICurrentUserService executionContextAccessor, IErpService erpService, INotificationService notificationService, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _executionContextAccessor = executionContextAccessor;
            _erpService = erpService;
            _notificationService = notificationService;
            _requestCancellationAccessor = requestCancellationAccessor;
        }
        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }
        private CancellationToken RequestCancellationToken => ResolveCancellationToken();


        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _executionContextAccessor.BranchCode ?? "0";
var entities = await _unitOfWork.SrtHeaders.Query().Where(x => x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _executionContextAccessor.BranchCode ?? "0";
var query = _unitOfWork.SrtHeaders.Query().Where(x => x.BranchCode == branchCode);

query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SrtHeaderDto>>(items);

var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data?.ToList() ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

var result = new PagedResponse<SrtHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SrtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SrtHeaderDto>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SrtHeaders.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    var notFound = _localizationService.GetLocalizedString("SrtHeaderNotFound");
    return ApiResponse<SrtHeaderDto>.ErrorResult(notFound, notFound, 404);
}
var dto = _mapper.Map<SrtHeaderDto>(entity);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(new[] { dto });
if (!enrichedCustomer.Success)
{
    return ApiResponse<SrtHeaderDto>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dto = enrichedCustomer.Data?.FirstOrDefault() ?? dto;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(new[] { dto });
if (!enrichedWarehouse.Success)
{
    return ApiResponse<SrtHeaderDto>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dto = enrichedWarehouse.Data?.FirstOrDefault() ?? dto;
return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByBranchCodeAsync(string branchCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SrtHeaders.Query().Where(x => x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _executionContextAccessor.BranchCode ?? "0";
var entities = await _unitOfWork.SrtHeaders.Query().Where(x => x.PlannedDate >= startDate && x.PlannedDate <= endDate && x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByCustomerCodeAsync(string customerCode, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _executionContextAccessor.BranchCode ?? "0";
var entities = await _unitOfWork.SrtHeaders.Query().Where(x => x.CustomerCode == customerCode && x.BranchCode == branchCode).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentTypeAsync(string documentType, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SrtHeaders.Query().Where(x => x.DocumentType == documentType).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetByDocumentNoAsync(string documentNo, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entities = await _unitOfWork.SrtHeaders.Query().Where(x => x.ERPReferenceNumber == documentNo).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<SrtHeaderDto>> CreateAsync(CreateSrtHeaderDto createDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (createDto == null)
{
    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("RequestOrHeaderMissing"), 400);
}
if (string.IsNullOrWhiteSpace(createDto.BranchCode) || string.IsNullOrWhiteSpace(createDto.DocumentType) || string.IsNullOrWhiteSpace(createDto.YearCode))
{
    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
}
if (createDto.YearCode?.Length != 4)
{
    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
}
if (createDto.PlannedDate == default)
{
    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), _localizationService.GetLocalizedString("HeaderFieldsMissing"), 400);
}
               var entity = _mapper.Map<SrtHeader>(createDto);
entity.CreatedDate = DateTimeProvider.Now;
entity.IsDeleted = false;
await _unitOfWork.SrtHeaders.AddAsync(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SrtHeaderDto>(entity);
return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderCreatedSuccessfully"));
        }

        public async Task<ApiResponse<SrtHeaderDto>> UpdateAsync(long id, UpdateSrtHeaderDto updateDto, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SrtHeaders.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null)
{
    var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
    return ApiResponse<SrtHeaderDto>.ErrorResult(nf, nf, 404);
}
_mapper.Map(updateDto, entity);
entity.UpdatedDate = DateTimeProvider.Now;
_unitOfWork.SrtHeaders.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
var dto = _mapper.Map<SrtHeaderDto>(entity);
return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var importLines = await _unitOfWork.SrtImportLines.Query().Where(x => x.HeaderId == id).ToListAsync(requestCancellationToken);
if (importLines.Any())
{
    var msg = _localizationService.GetLocalizedString("SrtHeaderImportLinesExist");
    return ApiResponse<bool>.ErrorResult(msg, msg, 400);
}
await _unitOfWork.SrtHeaders.SoftDelete(id);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);
return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtHeaderDeletedSuccessfully"));
        }

        public async Task<ApiResponse<bool>> CompleteAsync(long id, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.SrtHeaders.Query(tracking: true)
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null)
{
    var notFound = _localizationService.GetLocalizedString("SrtHeaderNotFound");
    return ApiResponse<bool>.ErrorResult(notFound, notFound, 404);
}

// ============================================
// CHECK ERP APPROVAL REQUIREMENT
// ============================================
var srtParameter = await _unitOfWork.SrtParameters
    .Query()
    .FirstOrDefaultAsync(requestCancellationToken);

// ============================================
// VALIDATE LINE SERIAL VS ROUTE QUANTITIES
// ============================================
// Skip validation only if both AllowLessQuantityBasedOnOrder and AllowMoreQuantityBasedOnOrder are true
// Normalize null values to false
bool skipValidation = (srtParameter?.AllowLessQuantityBasedOnOrder ?? false) 
    && (srtParameter?.AllowMoreQuantityBasedOnOrder ?? false);

// Normalize RequireAllOrderItemsCollected
bool requireAllOrderItemsCollected = srtParameter?.RequireAllOrderItemsCollected ?? false;

if (!skipValidation)
{
    var lines = await _unitOfWork.SrtLines
        .Query()
        .Where(l => l.HeaderId == id)
        .ToListAsync(requestCancellationToken);

    foreach (var line in lines)
    {
        // Get total quantity of LineSerials for this Line
        var totalLineSerialQuantity = await _unitOfWork.SrtLineSerials
            .Query()
            .Where(ls => ls.LineId == line.Id)
            .SumAsync(ls => ls.Quantity);

        // Get total quantity of Routes for ImportLines linked to this Line
        var totalRouteQuantity = await _unitOfWork.SrtRoutes
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
                var msg = _localizationService.GetLocalizedString("SrtHeaderAllOrderItemsMustBeCollected", 
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
        bool allowLess = srtParameter?.AllowLessQuantityBasedOnOrder ?? false;
        bool allowMore = srtParameter?.AllowMoreQuantityBasedOnOrder ?? false;
        
        bool quantityMismatch = false;
        string localizedMessage = string.Empty;
        string exceptionMessage = string.Empty;

        if (!allowLess && !allowMore)
        {
            // Both false: Exact match required (==)
            if (Math.Abs(totalLineSerialQuantity - totalRouteQuantity) > 0.000001m)
            {
                quantityMismatch = true;
                localizedMessage = _localizationService.GetLocalizedString("SrtHeaderQuantityExactMatchRequired", 
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
                localizedMessage = _localizationService.GetLocalizedString("SrtHeaderQuantityCannotBeGreater", 
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
                localizedMessage = _localizationService.GetLocalizedString("SrtHeaderQuantityCannotBeLess", 
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
    entity.IsPendingApproval = srtParameter != null && srtParameter.RequireApprovalBeforeErp;
    _unitOfWork.SrtHeaders.Update(entity);

    // Update package status to Shipped
    var package = _unitOfWork.PHeaders.Query(tracking: true)
        .Where(p => p.SourceHeaderId == entity.Id && p.SourceType == PHeaderSourceType.SRT)
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
            Title = _localizationService.GetLocalizedString("SrtDoneNotificationTitle", orderNumber),
            Message = _localizationService.GetLocalizedString("SrtDoneNotificationMessage", orderNumber),
            TitleKey = "SrtDoneNotificationTitle",
            MessageKey = "SrtDoneNotificationMessage",
            Channel = NotificationChannel.Web,
            Severity = NotificationSeverity.Info,
            RecipientUserId = entity.CreatedBy.Value,
            RelatedEntityType = NotificationEntityType.SRTDone,
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

    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("SrtHeaderCompletedSuccessfully"));
}
catch
{
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
        }

        public async Task<ApiResponse<IEnumerable<SrtHeaderDto>>> GetAssignedOrdersAsync(long userId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var branchCode = _executionContextAccessor.BranchCode ?? "0";

// Daha performanslı: Subquery kullanarak EXISTS benzeri kontrol
// SQL'de daha verimli bir sorgu üretir ve Distinct() gerektirmez
// Header ve TerminalLine'ın silinmemiş olduğunu kontrol eder
var query = _unitOfWork.SrtHeaders
    .Query()
    .Where(h => !h.IsCompleted 
        && h.BranchCode == branchCode
        && _unitOfWork.SrtTerminalLines
            .Query(false, false)
            .Any(t => t.HeaderId == h.Id 
                && t.TerminalUserId == userId));

var entities = await query.ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<IEnumerable<SrtHeaderDto>>(entities);
var enriched = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enriched.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enriched.Message, enriched.ExceptionMessage, enriched.StatusCode);
}
dtos = enriched.Data ?? dtos;
var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<IEnumerable<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data ?? dtos;
return ApiResponse<IEnumerable<SrtHeaderDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("SrtHeaderAssignedOrdersRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<PagedResponse<SrtHeaderDto>>> GetCompletedAwaitingErpApprovalPagedAsync(PagedRequest request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var query = _unitOfWork.SrtHeaders.Query()
    .Where(x => x.IsCompleted && x.IsPendingApproval && !x.IsERPIntegrated && x.ApprovalStatus == null);

query = query.ApplyFilters(request.Filters, request.FilterLogic);
bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
query = query.ApplySorting(request.SortBy ?? "Id", desc);

var totalCount = await query.CountAsync(requestCancellationToken);
var items = await query.ApplyPagination(request.PageNumber, request.PageSize).ToListAsync(requestCancellationToken);
var dtos = _mapper.Map<List<SrtHeaderDto>>(items);

var enrichedCustomer = await _erpService.PopulateCustomerNamesAsync(dtos);
if (!enrichedCustomer.Success)
{
    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedCustomer.Message, enrichedCustomer.ExceptionMessage, enrichedCustomer.StatusCode);
}
dtos = enrichedCustomer.Data?.ToList() ?? dtos;

var enrichedWarehouse = await _erpService.PopulateWarehouseNamesAsync(dtos);
if (!enrichedWarehouse.Success)
{
    return ApiResponse<PagedResponse<SrtHeaderDto>>.ErrorResult(enrichedWarehouse.Message, enrichedWarehouse.ExceptionMessage, enrichedWarehouse.StatusCode);
}
dtos = enrichedWarehouse.Data?.ToList() ?? dtos;

var result = new PagedResponse<SrtHeaderDto>(dtos, totalCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<SrtHeaderDto>>.SuccessResult(result, _localizationService.GetLocalizedString("SrtHeaderCompletedAwaitingErpApprovalRetrievedSuccessfully"));
        }
        public async Task<ApiResponse<SrtAssignedOrderLinesDto>> GetAssignedOrderLinesAsync(long headerId, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var lines = await _unitOfWork.SrtLines
    .Query()
    .Where(x => x.HeaderId == headerId)
    .ToListAsync(requestCancellationToken);

var lineIds = lines.Select(l => l.Id).ToList();

IEnumerable<SrtLineSerial> lineSerials = Array.Empty<SrtLineSerial>();
if (lineIds.Count > 0)
{
    lineSerials = await _unitOfWork.SrtLineSerials
        .Query()
        .Where(x => lineIds.Contains(x.LineId))
        .ToListAsync(requestCancellationToken);
}

var importLines = await _unitOfWork.SrtImportLines
    .Query()
    .Where(x => x.HeaderId == headerId)
    .ToListAsync(requestCancellationToken);

var importLineIds = importLines.Select(il => il.Id).ToList();

IEnumerable<SrtRoute> routes = Array.Empty<SrtRoute>();
if (importLineIds.Count > 0)
{
    routes = await _unitOfWork.SrtRoutes
        .Query()
        .Where(x => importLineIds.Contains(x.ImportLineId))
        .ToListAsync(requestCancellationToken);
}

var lineDtos = _mapper.Map<IEnumerable<SrtLineDto>>(lines);
if (lineDtos.Any())
{
    var enrichedLines = await _erpService.PopulateStockNamesAsync(lineDtos);
    if (enrichedLines.Success)
    {
        lineDtos = enrichedLines.Data ?? lineDtos;
    }
}

var importLineDtos = _mapper.Map<IEnumerable<SrtImportLineDto>>(importLines);
if (importLineDtos.Any())
{
    var enrichedImportLines = await _erpService.PopulateStockNamesAsync(importLineDtos);
    if (enrichedImportLines.Success)
    {
        importLineDtos = enrichedImportLines.Data ?? importLineDtos;
    }
}

var dto = new SrtAssignedOrderLinesDto
{
    Lines = lineDtos,
    LineSerials = _mapper.Map<IEnumerable<SrtLineSerialDto>>(lineSerials),
    ImportLines = importLineDtos,
    Routes = _mapper.Map<IEnumerable<SrtRouteDto>>(routes)
};

return ApiResponse<SrtAssignedOrderLinesDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderAssignedOrderLinesRetrievedSuccessfully"));
        }

        

        public async Task<ApiResponse<SrtHeaderDto>> SetApprovalAsync(long id, bool approved, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
// Tracking ile yükle (navigation property'ler yüklenmeyecek)
var entity = await _unitOfWork.SrtHeaders
    .Query(tracking: true)
    .Where(e => e.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
    
if (entity == null)
{
    var nf = _localizationService.GetLocalizedString("SrtHeaderNotFound");
    return ApiResponse<SrtHeaderDto>.ErrorResult(nf, nf, 404);
}

if (!(entity.IsCompleted && entity.IsPendingApproval && entity.ApprovalStatus == null))
{
    var msg = _localizationService.GetLocalizedString("SrtHeaderApprovalUpdateError");
    return ApiResponse<SrtHeaderDto>.ErrorResult(msg, msg, 400);
}

var approvedByUserId = _executionContextAccessor.UserId;

entity.ApprovalStatus = approved;
entity.ApprovedByUserId = approvedByUserId;
entity.ApprovalDate = DateTime.Now;
entity.IsPendingApproval = false;

_unitOfWork.SrtHeaders.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var dto = _mapper.Map<SrtHeaderDto>(entity);
return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderApprovalUpdatedSuccessfully"));
        }

        public async Task<ApiResponse<SrtHeaderDto>> GenerateOrderAsync(GenerateSubcontractingReceiptOrderRequestDto request, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
using (var tx = await _unitOfWork.BeginTransactionAsync())
{
    try
    {
        var header = _mapper.Map<SrtHeader>(request.Header) ?? new SrtHeader();
        await _unitOfWork.SrtHeaders.AddAsync(header);
        await _unitOfWork.SaveChangesAsync(requestCancellationToken);

        var lineKeyToId = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        var lineGuidToId = new Dictionary<Guid, long>();

        if (request.Lines != null && request.Lines.Count > 0)
        {
            var lines = new List<SrtLine>(request.Lines.Count);
            foreach (var l in request.Lines)
            {
                var line = _mapper.Map<SrtLine>(l);
                line.HeaderId = header.Id;
                lines.Add(line);
            }
            await _unitOfWork.SrtLines.AddRangeAsync(lines);
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
            var serials = new List<SrtLineSerial>(request.LineSerials.Count);
            foreach (var s in request.LineSerials)
            {
                long lineId = 0;
                if (s.LineGroupGuid.HasValue)
                {
                    var lg = s.LineGroupGuid.Value;
                    if (!lineGuidToId.TryGetValue(lg, out lineId))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("SrtHeaderLineGroupGuidNotFound"), 400);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(s.LineClientKey))
                {
                    if (!lineKeyToId.TryGetValue(s.LineClientKey!, out lineId))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("SrtHeaderLineClientKeyNotFound"), 400);
                    }
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return ApiResponse<SrtHeaderDto>.ErrorResult(_localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"), _localizationService.GetLocalizedString("SrtHeaderLineReferenceMissing"), 400);
                }

                var serial = _mapper.Map<SrtLineSerial>(s);
                serial.LineId = lineId;
                serials.Add(serial);
            }
            await _unitOfWork.SrtLineSerials.AddRangeAsync(serials);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);
        }

        List<Notification> createdNotifications = new List<Notification>();
        
        if (request.TerminalLines != null && request.TerminalLines.Count > 0)
        {
            var tlines = new List<SrtTerminalLine>(request.TerminalLines.Count);
            foreach (var t in request.TerminalLines)
            {
                var tline = _mapper.Map<SrtTerminalLine>(t);
                tline.HeaderId = header.Id;
                tlines.Add(tline);
            }
            await _unitOfWork.SrtTerminalLines.AddRangeAsync(tlines);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);

            // Create and add notifications for each terminal line
            var orderNumber = header.Id.ToString();
            createdNotifications = await _notificationService.CreateAndAddNotificationsForTerminalLinesAsync(
                tlines,
                orderNumber,
                NotificationEntityType.SRTHeader,
                "SRT_HEADER",
                "SrtHeaderNotificationTitle",
                "SrtHeaderNotificationMessage"
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

        var dto = _mapper.Map<SrtHeaderDto>(header);
        return ApiResponse<SrtHeaderDto>.SuccessResult(dto, _localizationService.GetLocalizedString("SrtHeaderGenerateCompletedSuccessfully"));
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync();
        throw;
    }
}
        }

        public async Task<ApiResponse<int>> BulkCreateSubcontractingReceiptTransferAsync(BulkCreateSrtRequestDto request, CancellationToken cancellationToken = default)
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
        var srtParameter = await _unitOfWork.SrtParameters
            .Query()
            .FirstOrDefaultAsync(requestCancellationToken);

        // ============================================
        // 2. CREATE HEADER
        // ============================================
        var header = _mapper.Map<SrtHeader>(request.Header);
        
        // Set IsPendingApproval: true if parameter exists and requires approval, otherwise false
        header.IsPendingApproval = srtParameter != null && srtParameter.RequireApprovalBeforeErp;

        await _unitOfWork.SrtHeaders.AddAsync(header);
        await _unitOfWork.SaveChangesAsync(requestCancellationToken);

        if (header.Id <= 0)
        {
            await tx.RollbackAsync();
            return ApiResponse<int>.ErrorResult(
                _localizationService.GetLocalizedString("SrtHeaderBulkCreateError"),
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
            var lines = new List<SrtLine>(request.Lines.Count);
            foreach (var lineDto in request.Lines)
            {
                var line = _mapper.Map<SrtLine>(lineDto) ?? new SrtLine();
                line.HeaderId = header.Id;
                lines.Add(line);
            }

            await _unitOfWork.SrtLines.AddRangeAsync(lines);
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
            var serials = new List<SrtLineSerial>(request.LineSerials.Count);
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderLineGroupGuidNotFound"),
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderLineClientKeyNotFound"),
                            400);
                    }
                    lineId = foundLineId;
                }
                else
                {
                    await tx.RollbackAsync();
                    return ApiResponse<int>.ErrorResult(
                        _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                        _localizationService.GetLocalizedString("LineReferenceMissing"),
                        400);
                }

                var serial = _mapper.Map<SrtLineSerial>(serialDto) ?? new SrtLineSerial();
                serial.LineId = lineId;
                serials.Add(serial);
            }

            await _unitOfWork.SrtLineSerials.AddRangeAsync(serials);
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
            var importLines = new List<SrtImportLine>(request.ImportLines.Count);
            foreach (var importDto in request.ImportLines)
            {
                // ClientKey is required for correlation
                if (string.IsNullOrWhiteSpace(importDto.ClientKey))
                {
                    await tx.RollbackAsync();
                    return ApiResponse<int>.ErrorResult(
                        _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderLineGroupGuidNotFound"),
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderLineClientKeyNotFound"),
                            400);
                    }
                    lineId = foundLineId;
                }

                var importLine = _mapper.Map<SrtImportLine>(importDto) ?? new SrtImportLine();
                importLine.HeaderId = header.Id;
                importLine.LineId = lineId;
                importLines.Add(importLine);
            }

            await _unitOfWork.SrtImportLines.AddRangeAsync(importLines);
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
            var routes = new List<SrtRoute>(request.Routes.Count);
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
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
                            _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                            _localizationService.GetLocalizedString("SrtHeaderRouteGroupGuidNotFound"),
                            400);
                    }
                    importLineId = foundImportLineId;
                }

                if (!importLineId.HasValue)
                {
                    await tx.RollbackAsync();
                    return ApiResponse<int>.ErrorResult(
                        _localizationService.GetLocalizedString("SrtHeaderInvalidCorrelationKey"),
                        _localizationService.GetLocalizedString("SrtHeaderLineReferenceMissing"),
                        400);
                }

                var route = _mapper.Map<SrtRoute>(routeDto) ?? new SrtRoute();
                route.ImportLineId = importLineId.Value;
                routes.Add(route);
            }

            await _unitOfWork.SrtRoutes.AddRangeAsync(routes);
            await _unitOfWork.SaveChangesAsync(requestCancellationToken);
        }

        // ============================================
        // 7. COMMIT TRANSACTION
        // ============================================
        await tx.CommitAsync();
        return ApiResponse<int>.SuccessResult(
            1,
            _localizationService.GetLocalizedString("SrtHeaderBulkCreateCompletedSuccessfully"));
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
