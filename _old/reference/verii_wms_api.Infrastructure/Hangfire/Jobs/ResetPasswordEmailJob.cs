using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Services.Jobs
{
    public class ResetPasswordEmailJob : IResetPasswordEmailJob
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ResetPasswordEmailJob> _logger;
        private readonly IMailService _mailService;

        public ResetPasswordEmailJob(IConfiguration configuration, ILogger<ResetPasswordEmailJob> logger, IMailService mailService)
        {
            _configuration = configuration;
            _logger = logger;
            _mailService = mailService;
        }

        [Queue("email")]
        public async Task SendUserCreatedEmailAsync(string email, string username, string password, string? firstName, string? lastName)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return;
            }

            var effectiveBaseUrl = GetFrontendBaseUrl();
            var emailSubject = "Kullanıcınız oluşturulmuştur";
            var displayName = string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName)
                ? username
                : $"{firstName} {lastName}".Trim();

            var content = $@"
                <p>Sayın {displayName},</p>
                <p>Kullanıcınız başarıyla oluşturulmuştur. Giriş bilgileriniz aşağıdadır:</p>
                <div class=""info-box"">
                    <p><strong>Login için E-postanız:</strong> {email}</p>
                    <p><strong>Şifreniz:</strong> {password}</p>
                </div>
                <p>Yukarıdaki bilgilerle giriş yapıp menü üzerinden kullanıcı şifrenizi değiştirebilirsiniz.</p>
                <div style=""text-align: center; margin-top: 30px;"">
                    <a href=""{effectiveBaseUrl}"" class=""btn"">Giriş Yap</a>
                </div>";

            var emailBody = GetEmailTemplate("Kullanıcınız Oluşturuldu", content);
            var isSent = await _mailService.SendEmailAsync(email, emailSubject, emailBody, true);
            if (!isSent)
            {
                _logger.LogWarning("User created e-mail could not be sent to {Email}", email);
            }
        }

        [Queue("email")]
        public async Task SendPasswordResetEmailAsync(string toEmail, string fullName, string token)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                return;
            }

            var baseUrl = GetFrontendBaseUrl();
            var resetPath = _configuration["FrontendSettings:ResetPasswordPath"]
                            ?? _configuration["Smtp:ResetPath"]
                            ?? "/reset-password";
            var link = $"{baseUrl.TrimEnd('/')}{resetPath}?token={token}";
            var subject = "Şifre Sıfırlama";
            var safeFullName = string.IsNullOrWhiteSpace(fullName) ? "Değerli Kullanıcı" : fullName;
            var content = $@"
                <p>Sayın {safeFullName},</p>
                <p>Şifre sıfırlama talebiniz alınmıştır. Aşağıdaki butona tıklayarak şifrenizi sıfırlayabilirsiniz:</p>
                <div style=""text-align: center; margin: 30px 0;"">
                    <a href=""{link}"" class=""btn"">Şifremi Sıfırla</a>
                </div>
                <p>Veya aşağıdaki linki tarayıcınıza kopyalayabilirsiniz:</p>
                <p style=""word-break: break-all; color: #fb923c; font-size: 14px;"">{link}</p>
                <div style=""margin-top: 20px; padding-top: 20px; border-top: 1px solid rgba(255,255,255,0.1);"">
                    <p style=""font-size: 13px; color: #94a3b8; margin: 0;"">Bu link 30 dakika süreyle geçerlidir.</p>
                    <p style=""font-size: 13px; color: #94a3b8; margin: 5px 0 0 0;"">Eğer şifre sıfırlama talebinde bulunmadıysanız, lütfen bu e-postayı dikkate almayınız.</p>
                </div>";
            var body = GetEmailTemplate("Şifre Sıfırlama Talebi", content);

            var isSent = await _mailService.SendEmailAsync(toEmail, subject, body, true);
            if (!isSent)
            {
                _logger.LogWarning("Password reset e-mail could not be sent to {Email}", toEmail);
            }
        }

        [Queue("email")]
        public async Task SendPasswordResetCompletedEmailAsync(string toEmail, string displayName)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                return;
            }

            var baseUrl = GetFrontendBaseUrl();
            var subject = "Şifre Sıfırlama İşlemi Tamamlandı";
            var safeDisplayName = string.IsNullOrWhiteSpace(displayName) ? "Değerli Kullanıcı" : displayName;
            var content = $@"
                <p>Sayın {safeDisplayName},</p>
                <p>Şifre resetleme işlemi başarılı şekilde tamamlanmıştır.</p>
                <p>Yeni şifreniz ile güvenli şekilde giriş yapabilirsiniz.</p>
                <div style=""text-align: center; margin-top: 30px;"">
                    <a href=""{baseUrl}"" class=""btn"">Giriş Yap</a>
                </div>";
            var body = GetEmailTemplate("Şifre Sıfırlama Tamamlandı", content);

            var isSent = await _mailService.SendEmailAsync(toEmail, subject, body, true);
            if (!isSent)
            {
                _logger.LogWarning("Password reset completed e-mail could not be sent to {Email}", toEmail);
            }
        }

        [Queue("email")]
        public async Task SendPasswordChangedEmailAsync(string toEmail, string displayName)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                return;
            }

            var baseUrl = GetFrontendBaseUrl();
            var subject = "Şifreniz Güncellendi";
            var safeDisplayName = string.IsNullOrWhiteSpace(displayName) ? "Değerli Kullanıcı" : displayName;
            var content = $@"
                <p>Sayın {safeDisplayName},</p>
                <p>Eski şifreniz başarılı şekilde güncellenmiştir.</p>
                <p>Bu işlemi siz yapmadıysanız lütfen hemen destek ekibi ile iletişime geçin.</p>
                <div style=""text-align: center; margin-top: 30px;"">
                    <a href=""{baseUrl}"" class=""btn"">Giriş Yap</a>
                </div>";
            var body = GetEmailTemplate("Şifre Güncelleme Bildirimi", content);

            var isSent = await _mailService.SendEmailAsync(toEmail, subject, body, true);
            if (!isSent)
            {
                _logger.LogWarning("Password changed e-mail could not be sent to {Email}", toEmail);
            }
        }

        private string GetFrontendBaseUrl()
        {
            return _configuration["FrontendSettings:BaseUrl"]?.TrimEnd('/')
                ?? _configuration["Smtp:ClientBaseUrl"]?.TrimEnd('/')
                ?? "https://wms.v3rii.com";
        }

        private string GetEmailTemplate(string title, string content)
        {
            var year = DateTime.Now.Year;
            return $@"
<!DOCTYPE html>
<html>
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<link href=""https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600;700&display=swap"" rel=""stylesheet"">
<style>
    body {{ font-family: 'Outfit', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #0f0518; margin: 0; padding: 0; color: #ffffff; }}
    .wrapper {{ width: 100%; table-layout: fixed; background-color: #0f0518; padding-bottom: 40px; }}
    .container {{ max-width: 600px; margin: 0 auto; background-color: #140a1e; border-radius: 24px; border: 1px solid rgba(255,255,255,0.1); overflow: hidden; box-shadow: 0 20px 40px rgba(0,0,0,0.4); }}
    .header {{ padding: 40px 40px 20px 40px; text-align: center; background: radial-gradient(circle at 50% -20%, rgba(236, 72, 153, 0.15), transparent 70%); }}
    .header h2 {{ margin: 0; font-size: 24px; font-weight: 700; color: #ffffff; text-transform: uppercase; letter-spacing: 1px; }}
    .content {{ padding: 20px 40px 40px 40px; color: #e2e8f0; line-height: 1.6; font-size: 16px; }}
    .footer {{ padding: 20px; text-align: center; color: #64748b; font-size: 12px; border-top: 1px solid rgba(255,255,255,0.05); background-color: #0c0516; }}
    .btn {{ display: inline-block; padding: 14px 32px; color: #ffffff !important; text-decoration: none; border-radius: 12px; font-weight: bold; text-transform: uppercase; letter-spacing: 1px; margin: 10px 5px; background: #f97316; background: linear-gradient(90deg, #db2777, #f97316, #eab308); box-shadow: 0 4px 15px rgba(249, 115, 22, 0.3); transition: all 0.3s ease; }}
    .btn:hover {{ opacity: 0.9; transform: translateY(-2px); box-shadow: 0 6px 20px rgba(249, 115, 22, 0.4); }}
    .btn-secondary {{ background: transparent; border: 1px solid rgba(255,255,255,0.2); color: #e2e8f0 !important; box-shadow: none; }}
    .btn-secondary:hover {{ background: rgba(255,255,255,0.05); border-color: rgba(255,255,255,0.4); }}
    .info-box {{ background-color: rgba(0,0,0,0.3); padding: 20px; border-radius: 12px; margin: 20px 0; border: 1px solid rgba(255,255,255,0.1); }}
    strong {{ color: #fb923c; }}
    a {{ color: #fb923c; text-decoration: none; }}
    p {{ margin-bottom: 15px; }}
</style>
</head>
<body>
    <div class=""wrapper"">
        <br>
        <div class=""container"">
            <div class=""header"">
                <h2>{title}</h2>
            </div>
            <div class=""content"">
                {content}
            </div>
            <div class=""footer"">
                <p>Bu e-posta otomatik olarak gönderilmiştir, lütfen yanıtlamayınız.</p>
                <p>&copy; {year} VERII WMS SYSTEM</p>
            </div>
        </div>
        <br>
    </div>
</body>
</html>";
        }
    }
}
