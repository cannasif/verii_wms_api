using System.Globalization;

namespace WMS_WEBAPI.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var languageHeader = context.Request.Headers["X-Language"].FirstOrDefault();
            var branchHeader = context.Request.Headers["X-Branch-Code"].FirstOrDefault();
            
            if (!string.IsNullOrEmpty(languageHeader))
            {
                try
                {
                    var culture = new CultureInfo(languageHeader);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                    
                    // Thread için de ayarla
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
                catch (CultureNotFoundException)
                {
                    // Geçersiz culture durumunda varsayılan olarak tr-TR kullan
                    var defaultCulture = new CultureInfo("tr-TR");
                    CultureInfo.CurrentCulture = defaultCulture;
                    CultureInfo.CurrentUICulture = defaultCulture;
                    Thread.CurrentThread.CurrentCulture = defaultCulture;
                    Thread.CurrentThread.CurrentUICulture = defaultCulture;
                }
            }
            else
            {
                // Header yoksa varsayılan olarak tr-TR kullan
                var defaultCulture = new CultureInfo("tr-TR");
                CultureInfo.CurrentCulture = defaultCulture;
                CultureInfo.CurrentUICulture = defaultCulture;
                Thread.CurrentThread.CurrentCulture = defaultCulture;
                Thread.CurrentThread.CurrentUICulture = defaultCulture;
            }

            // BranchCode bilgisi: varsa kullan, yoksa "0" olarak set et
            var branchCode = string.IsNullOrWhiteSpace(branchHeader) ? "0" : branchHeader;
            context.Items["BranchCode"] = branchCode;

            await _next(context);
        }
    }
}
