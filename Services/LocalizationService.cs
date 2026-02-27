using Microsoft.Extensions.Localization;
using WMS_WEBAPI.Interfaces;
using System.Globalization;

namespace WMS_WEBAPI.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IStringLocalizerFactory _localizerFactory;

        public LocalizationService(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
        }

        public string GetLocalizedString(string key)
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            var localizer = _localizerFactory.Create("Messages", "WMS_WEBAPI");
            
            // Mevcut culture'a göre localized string döndür
            using (new CultureScope(currentCulture))
            {
                return localizer[key];
            }
        }

        public string GetLocalizedString(string key, params object[] arguments)
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            var localizer = _localizerFactory.Create("Messages", "WMS_WEBAPI");
            
            // Mevcut culture'a göre localized string döndür
            using (new CultureScope(currentCulture))
            {
                return localizer[key, arguments];
            }
        }
    }

    // Culture scope helper class
    public class CultureScope : IDisposable
    {
        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUICulture;

        public CultureScope(string cultureName)
        {
            _originalCulture = CultureInfo.CurrentCulture;
            _originalUICulture = CultureInfo.CurrentUICulture;

            var culture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _originalCulture;
            CultureInfo.CurrentUICulture = _originalUICulture;
        }
    }
}
