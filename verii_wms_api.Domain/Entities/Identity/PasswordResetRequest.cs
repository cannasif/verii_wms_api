namespace WMS_WEBAPI.Models
{
    public class PasswordResetRequest : BaseEntity
    {
        public long UserId { get; set; }
        public User? User { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public string? RequestIp { get; set; }
        public string? UserAgent { get; set; }
    }
}
