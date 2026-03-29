using Wms.Application.Common;

namespace Wms.Modules.Definitions.Localization;

public sealed class DefinitionsLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["ParameterRetrievedSuccessfully"] = "Parameter retrieved successfully",
            ["ParameterCreatedSuccessfully"] = "Parameter created successfully",
            ["ParameterUpdatedSuccessfully"] = "Parameter updated successfully",
            ["ParameterDeletedSuccessfully"] = "Parameter deleted successfully",
            ["ParameterNotFound"] = "Parameter not found"
        },
        ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["ParameterRetrievedSuccessfully"] = "Parametreler basariyla getirildi",
            ["ParameterCreatedSuccessfully"] = "Parametre basariyla olusturuldu",
            ["ParameterUpdatedSuccessfully"] = "Parametre basariyla guncellendi",
            ["ParameterDeletedSuccessfully"] = "Parametre basariyla silindi",
            ["ParameterNotFound"] = "Parametre bulunamadi"
        },
        ["fr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["ParameterRetrievedSuccessfully"] = "Parameter retrieved successfully",
            ["ParameterCreatedSuccessfully"] = "Parameter created successfully",
            ["ParameterUpdatedSuccessfully"] = "Parameter updated successfully",
            ["ParameterDeletedSuccessfully"] = "Parameter deleted successfully",
            ["ParameterNotFound"] = "Parameter not found"
        }
    };
}
