using Wms.Application.Common;

namespace Wms.Modules.ServiceAllocation.Localization;

public sealed class ServiceAllocationLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } =
        new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ServiceCaseRetrievedSuccessfully"] = "Service case records retrieved successfully",
                ["ServiceCaseCreatedSuccessfully"] = "Service case created successfully",
                ["ServiceCaseUpdatedSuccessfully"] = "Service case updated successfully",
                ["ServiceCaseDeletedSuccessfully"] = "Service case deleted successfully",
                ["ServiceCaseNotFound"] = "Service case not found",
                ["ServiceCaseAlreadyExists"] = "Service case already exists",
                ["ServiceCaseTimelineRetrievedSuccessfully"] = "Service case timeline retrieved successfully",
                ["ServiceCaseLineRetrievedSuccessfully"] = "Service case lines retrieved successfully",
                ["ServiceCaseLineCreatedSuccessfully"] = "Service case line created successfully",
                ["ServiceCaseLineUpdatedSuccessfully"] = "Service case line updated successfully",
                ["ServiceCaseLineDeletedSuccessfully"] = "Service case line deleted successfully",
                ["ServiceCaseLineNotFound"] = "Service case line not found",
                ["ServiceCaseIdRequired"] = "Service case id is required",
                ["QuantityMustBeGreaterThanZero"] = "Quantity must be greater than zero",
                ["StockIdRequired"] = "Stock id is required",
                ["StockIdRequiredForMaterialLine"] = "Stock id is required for spare part and replacement product lines",
                ["AvailableQuantityCannotBeNegative"] = "Available quantity cannot be negative",
                ["AllocationRecomputedSuccessfully"] = "Allocation queue recomputed successfully",
                ["OrderAllocationLineRetrievedSuccessfully"] = "Order allocation records retrieved successfully",
                ["OrderAllocationLineCreatedSuccessfully"] = "Order allocation line created successfully",
                ["OrderAllocationLineUpdatedSuccessfully"] = "Order allocation line updated successfully",
                ["OrderAllocationLineDeletedSuccessfully"] = "Order allocation line deleted successfully",
                ["OrderAllocationLineNotFound"] = "Order allocation line not found",
                ["OrderAllocationLineAlreadyExists"] = "Order allocation line already exists",
                ["BusinessDocumentLinkRetrievedSuccessfully"] = "Business document links retrieved successfully",
                ["BusinessDocumentLinkCreatedSuccessfully"] = "Business document link created successfully",
                ["BusinessDocumentLinkUpdatedSuccessfully"] = "Business document link updated successfully",
                ["BusinessDocumentLinkDeletedSuccessfully"] = "Business document link deleted successfully",
                ["BusinessDocumentLinkNotFound"] = "Business document link not found",
                ["BusinessDocumentLinkAlreadyExists"] = "Business document link already exists"
            },
            ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["ServiceCaseRetrievedSuccessfully"] = "Servis kayıtları başarıyla getirildi",
                ["ServiceCaseCreatedSuccessfully"] = "Servis vakası başarıyla oluşturuldu",
                ["ServiceCaseUpdatedSuccessfully"] = "Servis vakası başarıyla güncellendi",
                ["ServiceCaseDeletedSuccessfully"] = "Servis vakası başarıyla silindi",
                ["ServiceCaseNotFound"] = "Servis vakası bulunamadı",
                ["ServiceCaseAlreadyExists"] = "Servis vakası zaten mevcut",
                ["ServiceCaseTimelineRetrievedSuccessfully"] = "Servis vaka zaman çizelgesi başarıyla getirildi",
                ["ServiceCaseLineRetrievedSuccessfully"] = "Servis kalemleri başarıyla getirildi",
                ["ServiceCaseLineCreatedSuccessfully"] = "Servis kalemi başarıyla oluşturuldu",
                ["ServiceCaseLineUpdatedSuccessfully"] = "Servis kalemi başarıyla güncellendi",
                ["ServiceCaseLineDeletedSuccessfully"] = "Servis kalemi başarıyla silindi",
                ["ServiceCaseLineNotFound"] = "Servis kalemi bulunamadı",
                ["ServiceCaseIdRequired"] = "Servis vaka id alanı zorunludur",
                ["QuantityMustBeGreaterThanZero"] = "Miktar sıfırdan büyük olmalıdır",
                ["StockIdRequired"] = "Stock id alanı zorunludur",
                ["StockIdRequiredForMaterialLine"] = "Yedek parça ve değişim ürünü kalemlerinde stock id zorunludur",
                ["AvailableQuantityCannotBeNegative"] = "Kullanılabilir miktar negatif olamaz",
                ["AllocationRecomputedSuccessfully"] = "Hakediş kuyruğu başarıyla yeniden hesaplandı",
                ["OrderAllocationLineRetrievedSuccessfully"] = "Hakediş kayıtları başarıyla getirildi",
                ["OrderAllocationLineCreatedSuccessfully"] = "Hakediş satırı başarıyla oluşturuldu",
                ["OrderAllocationLineUpdatedSuccessfully"] = "Hakediş satırı başarıyla güncellendi",
                ["OrderAllocationLineDeletedSuccessfully"] = "Hakediş satırı başarıyla silindi",
                ["OrderAllocationLineNotFound"] = "Hakediş satırı bulunamadı",
                ["OrderAllocationLineAlreadyExists"] = "Hakediş satırı zaten mevcut",
                ["BusinessDocumentLinkRetrievedSuccessfully"] = "Belge bağlantıları başarıyla getirildi",
                ["BusinessDocumentLinkCreatedSuccessfully"] = "Belge bağlantısı başarıyla oluşturuldu",
                ["BusinessDocumentLinkUpdatedSuccessfully"] = "Belge bağlantısı başarıyla güncellendi",
                ["BusinessDocumentLinkDeletedSuccessfully"] = "Belge bağlantısı başarıyla silindi",
                ["BusinessDocumentLinkNotFound"] = "Belge bağlantısı bulunamadı",
                ["BusinessDocumentLinkAlreadyExists"] = "Belge bağlantısı zaten mevcut"
            }
        };
}
