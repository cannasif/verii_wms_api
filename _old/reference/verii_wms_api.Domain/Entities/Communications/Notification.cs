using System;

namespace WMS_WEBAPI.Models
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Localization key for Title (e.g., "WtHeaderNotificationTitle")
        /// If set, Title will be localized when returned to frontend
        /// </summary>
        public string? TitleKey { get; set; }

        /// <summary>
        /// Localization key for Message (e.g., "WtHeaderNotificationMessage")
        /// If set, Message will be localized when returned to frontend
        /// </summary>
        public string? MessageKey { get; set; }

        public NotificationChannel Channel { get; set; }

        public NotificationSeverity? Severity { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime? ReadDate { get; set; }

        public DateTime? ScheduledAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public long? RecipientUserId { get; set; }
        public virtual User? RecipientUser { get; set; }

        public string? RelatedEntityType { get; set; }

        public long? RelatedEntityId { get; set; }

        public string? ActionUrl { get; set; }

        public string? TerminalActionCode { get; set; }
    }
}
