using Wms.Application.Common;

namespace Wms.Modules.InventoryCount.Localization;

public sealed class InventoryCountLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["IcHeaderCreatedSuccessfully"] = "Ic Header Created Successfully",
            ["IcHeaderDeletedSuccessfully"] = "Ic Header Deleted Successfully",
            ["IcHeaderImportLinesExist"] = "Ic Header Import Lines Exist",
            ["IcHeaderNotFound"] = "Ic Header Not Found",
            ["IcHeaderRetrievedSuccessfully"] = "Ic Header Retrieved Successfully",
            ["IcHeaderUpdatedSuccessfully"] = "Ic Header Updated Successfully",
            ["IcImportLineCreatedSuccessfully"] = "Ic Import Line Created Successfully",
            ["IcImportLineDeletedSuccessfully"] = "Ic Import Line Deleted Successfully",
            ["IcImportLineNotFound"] = "Ic Import Line Not Found",
            ["IcImportLineRetrievedSuccessfully"] = "Ic Import Line Retrieved Successfully",
            ["IcImportLineRoutesExist"] = "Ic Import Line Routes Exist",
            ["IcImportLineUpdatedSuccessfully"] = "Ic Import Line Updated Successfully",
            ["IcRouteCreatedSuccessfully"] = "Ic Route Created Successfully",
            ["IcRouteDeletedSuccessfully"] = "Ic Route Deleted Successfully",
            ["IcRouteNotFound"] = "Ic Route Not Found",
            ["IcRouteRetrievedSuccessfully"] = "Ic Route Retrieved Successfully",
            ["IcRouteUpdatedSuccessfully"] = "Ic Route Updated Successfully",
            ["IcTerminalLineCreatedSuccessfully"] = "Ic Terminal Line Created Successfully",
            ["IcTerminalLineDeletedSuccessfully"] = "Ic Terminal Line Deleted Successfully",
            ["IcTerminalLineNotFound"] = "Ic Terminal Line Not Found",
            ["IcTerminalLineRetrievedSuccessfully"] = "Ic Terminal Line Retrieved Successfully",
            ["IcTerminalLineUpdatedSuccessfully"] = "Ic Terminal Line Updated Successfully"
        },
        ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["IcHeaderCreatedSuccessfully"] = "sayim basligi basariyla olusturuldu",
            ["IcHeaderDeletedSuccessfully"] = "sayim basligi basariyla silindi",
            ["IcHeaderImportLinesExist"] = "Icerisinde aktarim satiri bulunan sayim basligi silinemez",
            ["IcHeaderNotFound"] = "sayim basligi bulunamadi",
            ["IcHeaderRetrievedSuccessfully"] = "sayim basligi basariyla getirildi",
            ["IcHeaderUpdatedSuccessfully"] = "sayim basligi basariyla guncellendi",
            ["IcImportLineCreatedSuccessfully"] = "sayim aktarim satiri basariyla olusturuldu",
            ["IcImportLineDeletedSuccessfully"] = "sayim aktarim satiri basariyla silindi",
            ["IcImportLineNotFound"] = "sayim aktarim satiri bulunamadi",
            ["IcImportLineRetrievedSuccessfully"] = "sayim aktarim satiri basariyla getirildi",
            ["IcImportLineRoutesExist"] = "Bagli rota bulunan sayim aktarim satiri silinemez",
            ["IcImportLineUpdatedSuccessfully"] = "sayim aktarim satiri basariyla guncellendi",
            ["IcRouteCreatedSuccessfully"] = "sayim rotasi basariyla olusturuldu",
            ["IcRouteDeletedSuccessfully"] = "sayim rotasi basariyla silindi",
            ["IcRouteNotFound"] = "sayim rotasi bulunamadi",
            ["IcRouteRetrievedSuccessfully"] = "sayim rotasi basariyla getirildi",
            ["IcRouteUpdatedSuccessfully"] = "sayim rotasi basariyla guncellendi",
            ["IcTerminalLineCreatedSuccessfully"] = "sayim terminal satiri basariyla olusturuldu",
            ["IcTerminalLineDeletedSuccessfully"] = "sayim terminal satiri basariyla silindi",
            ["IcTerminalLineNotFound"] = "sayim terminal satiri bulunamadi",
            ["IcTerminalLineRetrievedSuccessfully"] = "sayim terminal satiri basariyla getirildi",
            ["IcTerminalLineUpdatedSuccessfully"] = "sayim terminal satiri basariyla guncellendi"
        },
        ["fr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["IcHeaderCreatedSuccessfully"] = "Ic Header Created Successfully",
            ["IcHeaderDeletedSuccessfully"] = "Ic Header Deleted Successfully",
            ["IcHeaderImportLinesExist"] = "Ic Header Import Lines Exist",
            ["IcHeaderNotFound"] = "Ic Header Not Found",
            ["IcHeaderRetrievedSuccessfully"] = "Ic Header Retrieved Successfully",
            ["IcHeaderUpdatedSuccessfully"] = "Ic Header Updated Successfully",
            ["IcImportLineCreatedSuccessfully"] = "Ic Import Line Created Successfully",
            ["IcImportLineDeletedSuccessfully"] = "Ic Import Line Deleted Successfully",
            ["IcImportLineNotFound"] = "Ic Import Line Not Found",
            ["IcImportLineRetrievedSuccessfully"] = "Ic Import Line Retrieved Successfully",
            ["IcImportLineRoutesExist"] = "Ic Import Line Routes Exist",
            ["IcImportLineUpdatedSuccessfully"] = "Ic Import Line Updated Successfully",
            ["IcRouteCreatedSuccessfully"] = "Ic Route Created Successfully",
            ["IcRouteDeletedSuccessfully"] = "Ic Route Deleted Successfully",
            ["IcRouteNotFound"] = "Ic Route Not Found",
            ["IcRouteRetrievedSuccessfully"] = "Ic Route Retrieved Successfully",
            ["IcRouteUpdatedSuccessfully"] = "Ic Route Updated Successfully",
            ["IcTerminalLineCreatedSuccessfully"] = "Ic Terminal Line Created Successfully",
            ["IcTerminalLineDeletedSuccessfully"] = "Ic Terminal Line Deleted Successfully",
            ["IcTerminalLineNotFound"] = "Ic Terminal Line Not Found",
            ["IcTerminalLineRetrievedSuccessfully"] = "Ic Terminal Line Retrieved Successfully",
            ["IcTerminalLineUpdatedSuccessfully"] = "Ic Terminal Line Updated Successfully"
        }
    };
}
