using Hangfire.Dashboard;

namespace WMS_WEBAPI
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // Development ortamında herkese izin ver
            // Production'da burayı JWT token kontrolü ile değiştirin
            return true;
        }
    }
}