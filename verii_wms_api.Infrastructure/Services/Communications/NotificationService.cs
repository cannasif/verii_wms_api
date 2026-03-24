using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.UnitOfWork;
using Microsoft.AspNetCore.SignalR;
using WMS_WEBAPI.Hubs;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILocalizationService localizationService, IHubContext<NotificationHub> notificationHub, IRequestCancellationAccessor requestCancellationAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _notificationHub = notificationHub;
            _requestCancellationAccessor = requestCancellationAccessor;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken token = default)
        {
            return _requestCancellationAccessor.Get(token);
        }

        public async Task<ApiResponse<NotificationDto>> CreateAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = _mapper.Map<Notification>(dto);
entity.DeliveredAt = DateTimeProvider.Now; // Auto-set delivery time

await _unitOfWork.Notifications.AddAsync(entity, requestCancellationToken);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

var result = LocalizeNotification(_mapper.Map<NotificationDto>(entity));

await PublishSignalRNotificationAsync(entity, requestCancellationToken);
return ApiResponse<NotificationDto>.SuccessResult(result, _localizationService.GetLocalizedString("NotificationCreatedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<NotificationDto>>> CreateForUsersAsync(CreateNotificationDto dto, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
using var tx = await _unitOfWork.BeginTransactionAsync(requestCancellationToken);
var recipients = dto.RecipientUserIds ?? new List<long>();
if (dto.RecipientUserId.HasValue)
{
    recipients.Add(dto.RecipientUserId.Value);
}

if (recipients.Count == 0)
{
    return ApiResponse<IEnumerable<NotificationDto>>.ErrorResult(_localizationService.GetLocalizedString("NotificationRecipientsRequired"), _localizationService.GetLocalizedString("NotificationRecipientsRequired"), 400);
}

try
{
    var entities = new List<Notification>();
    foreach (var userId in recipients.Distinct())
    {
        var entity = _mapper.Map<Notification>(dto);
        entity.RecipientUserId = userId;
        entity.DeliveredAt = DateTimeProvider.Now; // Auto-set delivery time
        entities.Add(entity);
    }

    await _unitOfWork.Notifications.AddRangeAsync(entities, requestCancellationToken);
    await _unitOfWork.SaveChangesAsync(requestCancellationToken);

    await _unitOfWork.CommitTransactionAsync(requestCancellationToken);

    foreach (var entity in entities)
    {
        await PublishSignalRNotificationAsync(entity, requestCancellationToken);
    }

    var dtos = LocalizeNotifications(_mapper.Map<List<NotificationDto>>(entities));
    return ApiResponse<IEnumerable<NotificationDto>>.SuccessResult(dtos, _localizationService.GetLocalizedString("NotificationBulkCreatedSuccessfully"));
}
catch
{
    await _unitOfWork.RollbackTransactionAsync(requestCancellationToken);
    throw;
}
        }

        public async Task<ApiResponse<PagedResponse<NotificationDto>>> GetPagedByRecipientUserIdAsync(long userId, PagedRequest request, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (request.PageNumber < 1) request.PageNumber = 1;
if (request.PageSize < 1) request.PageSize = 20;

var baseQuery = _unitOfWork.Notifications.Query()
    .Where(x => x.RecipientUserId == userId && !x.IsDeleted);

baseQuery = baseQuery.ApplyFilters(request.Filters, request.FilterLogic);

// Get unread count (totalCount will be unread notifications count)
var unreadCount = await baseQuery
    .Where(x => !x.IsRead)
    .CountAsync(requestCancellationToken);

// Sort: Unread first, then by Id descending (newest first)
var query = baseQuery
    .OrderBy(x => x.IsRead)            // false (unread) comes first, then true (read)
    .ThenByDescending(x => x.Id);      // Within each group, newest first

var items = await query
    .ApplyPagination(request.PageNumber, request.PageSize)
    .ToListAsync(requestCancellationToken);

var dtos = _mapper.Map<List<NotificationDto>>(items);
dtos = LocalizeNotifications(dtos);

var result = new PagedResponse<NotificationDto>(dtos, unreadCount, request.PageNumber, request.PageSize);
return ApiResponse<PagedResponse<NotificationDto>>.SuccessResult(result, _localizationService.GetLocalizedString("NotificationRetrievedSuccessfully"));
        }


        public async Task<ApiResponse<bool>> MarkAsReadAsync(long id, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
var entity = await _unitOfWork.Notifications.Query()
    .Where(x => x.Id == id)
    .FirstOrDefaultAsync(requestCancellationToken);
if (entity == null || entity.IsDeleted)
{
    return ApiResponse<bool>.ErrorResult(_localizationService.GetLocalizedString("NotificationNotFound"), _localizationService.GetLocalizedString("NotificationNotFound"), 404);
}

entity.IsRead = true;
entity.ReadDate = DateTime.UtcNow;
_unitOfWork.Notifications.Update(entity);
await _unitOfWork.SaveChangesAsync(requestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("NotificationMarkedReadSuccessfully"));
        }

        public async Task<ApiResponse<bool>> MarkAsReadBulkAsync(List<long> ids, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
if (ids == null || ids.Count == 0)
{
    return ApiResponse<bool>.ErrorResult(
        _localizationService.GetLocalizedString("InvalidModelState"),
        _localizationService.GetLocalizedString("NotificationIdsRequired"),
        400);
}

// Get all notifications that are not deleted and match the provided IDs
var entities = await _unitOfWork.Notifications.Query()
    .Where(x => ids.Contains(x.Id) && !x.IsDeleted)
    .ToListAsync(requestCancellationToken);

if (entities == null || !entities.Any())
{
    return ApiResponse<bool>.ErrorResult(
        _localizationService.GetLocalizedString("NotificationNotFound"),
        _localizationService.GetLocalizedString("NotificationValidRecordsNotFound"),
        404);
}

var now = DateTime.UtcNow;
foreach (var entity in entities)
{
    entity.IsRead = true;
    entity.ReadDate = now;
    entity.UpdatedDate = now;
    _unitOfWork.Notifications.Update(entity);
}

await _unitOfWork.SaveChangesAsync(requestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("NotificationMarkedReadSuccessfully"));
        }

        public async Task<ApiResponse<bool>> MarkAllAsReadAsync(long userId, CancellationToken cancellationToken = default)
        {
var requestCancellationToken = ResolveCancellationToken(cancellationToken);
// Get all unread notifications for the user
var entities = await _unitOfWork.Notifications.Query()
    .Where(x => x.RecipientUserId == userId && !x.IsDeleted && !x.IsRead)
    .ToListAsync(requestCancellationToken);

if (entities == null || !entities.Any())
{
    return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("NotificationMarkedReadSuccessfully"));
}

var now = DateTime.UtcNow;
foreach (var entity in entities)
{
    entity.IsRead = true;
    entity.ReadDate = now;
    entity.UpdatedDate = now;
    _unitOfWork.Notifications.Update(entity);
}

await _unitOfWork.SaveChangesAsync(requestCancellationToken);

return ApiResponse<bool>.SuccessResult(true, _localizationService.GetLocalizedString("NotificationMarkedReadSuccessfully"));
        }

   
        /// <summary>
        /// Creates notification entities for terminal lines without transaction or SaveChanges.
        /// Caller is responsible for adding to UnitOfWork and committing transaction.
        /// </summary>
        public List<Notification> CreateNotificationsForTerminalLines<TTerminalLine>(
            IEnumerable<TTerminalLine> terminalLines,
            string orderNumber,
            string entityType,
            string terminalActionCode,
            string titleLocalizationKey,
            string messageLocalizationKey) where TTerminalLine : class
        {
            var notifications = new List<Notification>();
            
            foreach (var tline in terminalLines)
            {
                // Use reflection to get TerminalUserId and Id properties
                var terminalUserIdProp = typeof(TTerminalLine).GetProperty("TerminalUserId");
                var idProp = typeof(TTerminalLine).GetProperty("Id");
                
                if (terminalUserIdProp == null || idProp == null)
                {
                    continue;
                }

                var terminalUserId = (long)terminalUserIdProp.GetValue(tline)!;
                var kayitId = long.Parse(orderNumber);

                var notification = new Notification
                {
                    // Store localization keys instead of localized strings
                    // Title and Message will be localized when returned to frontend
                    Title = titleLocalizationKey, // Store key, will be localized on retrieval
                    Message = messageLocalizationKey, // Store key, will be localized on retrieval
                    TitleKey = titleLocalizationKey,
                    MessageKey = messageLocalizationKey,
                    Channel = NotificationChannel.Terminal,
                    Severity = NotificationSeverity.Info,
                    RecipientUserId = terminalUserId,
                    RelatedEntityType = entityType,
                    RelatedEntityId = kayitId, // RelatedEntityId contains the order number (header ID)
                    TerminalActionCode = terminalActionCode,
                    DeliveredAt = DateTimeProvider.Now
                };
                notifications.Add(notification);
            }

            return notifications;
        }

        /// <summary>
        /// Publishes SignalR notifications for the given notification entities.
        /// Should be called after transaction is committed.
        /// </summary>
        public async Task PublishSignalRNotificationsAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            foreach (var entity in notifications)
            {
                await PublishSignalRNotificationAsync(entity, requestCancellationToken);
            }
        }

        /// <summary>
        /// Creates notifications for terminal lines and adds them to UnitOfWork.
        /// Caller must call SaveChangesAsync before committing transaction.
        /// After transaction is committed, call PublishSignalRNotificationsForCreatedNotificationsAsync to publish SignalR notifications.
        /// Returns the created notification entities so caller can track them.
        /// </summary>
        public async Task<List<Notification>> CreateAndAddNotificationsForTerminalLinesAsync<TTerminalLine>(
            IEnumerable<TTerminalLine> terminalLines,
            string orderNumber,
            string entityType,
            string terminalActionCode,
            string titleLocalizationKey,
            string messageLocalizationKey,
            CancellationToken cancellationToken = default) where TTerminalLine : class
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            var notifications = CreateNotificationsForTerminalLines(
                terminalLines,
                orderNumber,
                entityType,
                terminalActionCode,
                titleLocalizationKey,
                messageLocalizationKey
            );

            if (notifications.Count > 0)
            {
                await _unitOfWork.Notifications.AddRangeAsync(notifications, requestCancellationToken);
                // Note: Caller must call SaveChangesAsync before committing transaction
            }

            return notifications;
        }

        /// <summary>
        /// Publishes SignalR notifications for the given notification entities.
        /// Should be called after transaction is committed so notification IDs are available.
        /// </summary>
        public async Task PublishSignalRNotificationsForCreatedNotificationsAsync(List<Notification> notifications, CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = ResolveCancellationToken(cancellationToken);
            // After commit, notification IDs are assigned, so we can publish them
            if (notifications.Count == 0) return;

            // Get recipient user IDs and related entity IDs from notifications for more precise matching
            var recipientUserIds = notifications
                .Where(n => n.RecipientUserId.HasValue)
                .Select(n => n.RecipientUserId!.Value)
                .Distinct()
                .ToList();

            var relatedEntityIds = notifications
                .Where(n => n.RelatedEntityId.HasValue)
                .Select(n => n.RelatedEntityId!.Value)
                .Distinct()
                .ToList();

            if (recipientUserIds.Count == 0) return;

            // Get a time threshold (2 minutes ago) to find recently created notifications
            // This gives enough time for transaction commit and database write
            var timeThreshold = DateTime.UtcNow.AddMinutes(-2);

            // Fetch notifications from database by recipient user IDs, related entity IDs, and recent delivery time
            var savedNotifications = await _unitOfWork.Notifications
                .FindAsync(n => !n.IsDeleted
                    && n.DeliveredAt.HasValue
                    && n.DeliveredAt.Value >= timeThreshold
                    && n.RecipientUserId.HasValue
                    && recipientUserIds.Contains(n.RecipientUserId.Value)
                    && (relatedEntityIds.Count == 0 || (n.RelatedEntityId.HasValue && relatedEntityIds.Contains(n.RelatedEntityId.Value)))
                , requestCancellationToken);

            // Publish all matching notifications
            foreach (var notification in savedNotifications)
            {
                await PublishSignalRNotificationAsync(notification, requestCancellationToken);
            }
        }

        private async Task PublishSignalRNotificationAsync(Notification entity, CancellationToken cancellationToken = default)
        {
cancellationToken.ThrowIfCancellationRequested();
var type = entity.Severity switch
{
    NotificationSeverity.Info => "info",
    NotificationSeverity.Warning => "warning",
    NotificationSeverity.Error => "error",
    _ => "info"
};

// Localize title and message if keys are present
string localizedTitle = entity.Title;
string localizedMessage = entity.Message;

if (!string.IsNullOrEmpty(entity.TitleKey))
{
    // Get order number from RelatedEntityId
    var orderNumber = entity.RelatedEntityId?.ToString() ?? string.Empty;
    localizedTitle = _localizationService.GetLocalizedString(entity.TitleKey, orderNumber);
}

if (!string.IsNullOrEmpty(entity.MessageKey))
{
    // Get order number from RelatedEntityId
    var orderNumber = entity.RelatedEntityId?.ToString() ?? string.Empty;
    localizedMessage = _localizationService.GetLocalizedString(entity.MessageKey, orderNumber);
}

var payload = new
{
    id = entity.Id,
    title = localizedTitle,
    message = localizedMessage,
    type,
    timestamp = DateTime.UtcNow,
    channel = entity.Channel,
    recipientUserId = entity.RecipientUserId,
    relatedEntityType = entity.RelatedEntityType,
    relatedEntityId = entity.RelatedEntityId,
};

// Send to user (if specified)
if (entity.RecipientUserId.HasValue)
{
    await NotificationHub.SendNotificationToUser(_notificationHub, entity.RecipientUserId.Value.ToString(), payload);
}
else
{
    // If no specific recipient, send to all
    await NotificationHub.SendNotificationToAll(_notificationHub, payload);
}
        }

        /// <summary>
        /// Localizes a single notification DTO using TitleKey and MessageKey
        /// </summary>
        private NotificationDto LocalizeNotification(NotificationDto dto)
        {
            if (dto == null) return dto!;

// If TitleKey exists, localize Title using RelatedEntityId as order number
if (!string.IsNullOrEmpty(dto.TitleKey))
{
    var orderNumber = dto.RelatedEntityId?.ToString() ?? string.Empty;
    dto.Title = _localizationService.GetLocalizedString(dto.TitleKey, orderNumber);
}

// If MessageKey exists, localize Message using RelatedEntityId as order number
if (!string.IsNullOrEmpty(dto.MessageKey))
{
    var orderNumber = dto.RelatedEntityId?.ToString() ?? string.Empty;
    dto.Message = _localizationService.GetLocalizedString(dto.MessageKey, orderNumber);
}

            return dto;
        }

        /// <summary>
        /// Localizes a list of notification DTOs
        /// </summary>
        private List<NotificationDto> LocalizeNotifications(List<NotificationDto> dtos)
        {
            if (dtos == null) return new List<NotificationDto>();

            foreach (var dto in dtos)
            {
                LocalizeNotification(dto);
            }

            return dtos;
        }
    }
}
