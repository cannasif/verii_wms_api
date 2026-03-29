namespace WMS_WEBAPI.Interfaces
{
    public interface IMailService
    {
        Task<bool> SendEmailAsync(
            string to,
            string subject,
            string body,
            bool isHtml = true,
            string? cc = null,
            string? bcc = null,
            List<string>? attachments = null,
            CancellationToken cancellationToken = default);

        Task<bool> SendEmailAsync(
            string to,
            string subject,
            string body,
            string? fromEmail = null,
            string? fromName = null,
            bool isHtml = true,
            string? cc = null,
            string? bcc = null,
            List<string>? attachments = null,
            CancellationToken cancellationToken = default);
    }
}
