namespace WMS_WEBAPI.Models
{
    public class SmtpSetting : BaseEntity
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = string.Empty;
        public string PasswordEncrypted { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = "VERII WMS";
        public int Timeout { get; set; } = 30;
    }
}
