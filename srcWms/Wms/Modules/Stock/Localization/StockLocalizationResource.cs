using Wms.Application.Common;

namespace Wms.Modules.Stock.Localization;

public sealed class StockLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["StockRetrievedSuccessfully"] = "Stock records retrieved successfully",
            ["StockCreatedSuccessfully"] = "Stock created successfully",
            ["StockUpdatedSuccessfully"] = "Stock updated successfully",
            ["StockDeletedSuccessfully"] = "Stock deleted successfully",
            ["StockNotFound"] = "Stock not found",
            ["StockAlreadyExists"] = "ERP stock code already exists",
            ["StockSyncCompletedSuccessfully"] = "Stock sync completed successfully"
        },
        ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["StockRetrievedSuccessfully"] = "Stok kayıtları başarıyla getirildi",
            ["StockCreatedSuccessfully"] = "Stok başarıyla oluşturuldu",
            ["StockUpdatedSuccessfully"] = "Stok başarıyla güncellendi",
            ["StockDeletedSuccessfully"] = "Stok başarıyla silindi",
            ["StockNotFound"] = "Stok bulunamadı",
            ["StockAlreadyExists"] = "ERP stok kodu zaten mevcut",
            ["StockSyncCompletedSuccessfully"] = "Stok senkronizasyonu başarıyla tamamlandı"
        }
    };
}
