using Wms.Application.Common;

namespace Wms.Modules.Package.Localization;

public sealed class PackageLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["InvalidSourceType"] = "Invalid source type",
            ["UnsupportedMappingSourceType"] = "This source type is not supported yet in Wms",
            ["AvailableHeadersRetrievedSuccessfully"] = "Available headers retrieved successfully",
            ["SourceHeaderIdNotFound"] = "Source header id not found",
            ["SourceHeaderNotFound"] = "Source header not found",
            ["MatchedSourceHeaderMustBeActiveAndIncomplete"] = "Matched source header must be active and incomplete"
        },
        ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["InvalidSourceType"] = "Gecersiz kaynak tipi",
            ["UnsupportedMappingSourceType"] = "Bu kaynak tipi henuz desteklenmiyor",
            ["AvailableHeadersRetrievedSuccessfully"] = "Uygun basliklar basariyla getirildi",
            ["SourceHeaderIdNotFound"] = "Kaynak baslik kimligi bulunamadi",
            ["SourceHeaderNotFound"] = "Kaynak baslik bulunamadi",
            ["MatchedSourceHeaderMustBeActiveAndIncomplete"] = "Eslestirilen kaynak baslik aktif ve tamamlanmamis olmalidir"
        },
        ["fr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["InvalidSourceType"] = "Invalid source type",
            ["UnsupportedMappingSourceType"] = "This source type is not supported yet in Wms",
            ["AvailableHeadersRetrievedSuccessfully"] = "Available headers retrieved successfully",
            ["SourceHeaderIdNotFound"] = "Source header id not found",
            ["SourceHeaderNotFound"] = "Source header not found",
            ["MatchedSourceHeaderMustBeActiveAndIncomplete"] = "Matched source header must be active and incomplete"
        }
    };
}
