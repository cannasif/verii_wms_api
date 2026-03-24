using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IErpService
    {
        // OnHandQuantity işlemleri
        Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(int? depoKodu = null, string? stokKodu = null, string? seriNo = null, string? projeKodu = null, CancellationToken cancellationToken = default);

        // Cari işlemleri
        Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<T>>> PopulateCustomerNamesAsync<T>(IEnumerable<T> dtos, CancellationToken cancellationToken = default);

        // Stok işlemleri
        Task<ApiResponse<List<StokDto>>> GetStoksAsync(string? stokKodu, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> dtos, CancellationToken cancellationToken = default);

        // Depo işlemleri
        Task<ApiResponse<List<DepoDto>>> GetDeposAsync(short? depoKodu, CancellationToken cancellationToken = default);
        Task<ApiResponse<IEnumerable<T>>> PopulateWarehouseNamesAsync<T>(IEnumerable<T> dtos, CancellationToken cancellationToken = default);

        // Proje işlemleri
        Task<ApiResponse<List<ProjeDto>>> GetProjelerAsync(CancellationToken cancellationToken = default);

        // Stok barkod işlemleri
        Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(string bar, int depoKodu, int modul, int kullaniciId, string barkodGrubu, int hareketTuru, CancellationToken cancellationToken = default);

        // Şube işlemleri
        Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null, CancellationToken cancellationToken = default);

        // Depo ve Raf işlemleri
        Task<ApiResponse<List<WarehouseAndShelvesDto>>> GetWarehouseAndShelvesAsync(string? depoKodu = null, string? raf = null, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>> GetWarehouseShelvesWithStockInformationAsync(string? depoKodu = null, string? raf = null, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<WarehouseShelfStocksDto>>> GetWarehouseShelvesNestedAsync(string depoKodu, CancellationToken cancellationToken = default);

        // Üretim Emri işlemleri
        Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default);
        Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default);

    }
}
