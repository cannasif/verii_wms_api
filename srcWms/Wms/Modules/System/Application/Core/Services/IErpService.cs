using Wms.Application.Common; using Wms.Application.System.Dtos; using Wms.Application.Production.Dtos;
namespace Wms.Application.System.Services;
public interface IErpService
{
Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(int? depoKodu = null, string? stokKodu = null, string? seriNo = null, string? projeKodu = null, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<CariDto>>> GetCarisPagedAsync(PagedRequest request, string? cariKodu, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<StokDto>>> GetStoksPagedAsync(PagedRequest request, string? stokKodu, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<DepoDto>>> GetDeposPagedAsync(PagedRequest request, short? depoKodu, CancellationToken cancellationToken = default);
Task<ApiResponse<PagedResponse<ProjeDto>>> GetProjelerPagedAsync(PagedRequest request, CancellationToken cancellationToken = default);
Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(string bar, int depoKodu, int modul, int kullaniciId, string barkodGrubu, int hareketTuru, CancellationToken cancellationToken = default);
Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null, CancellationToken cancellationToken = default);
Task<ApiResponse<List<WarehouseAndShelvesDto>>> GetWarehouseAndShelvesAsync(string? depoKodu = null, string? raf = null, CancellationToken cancellationToken = default);
Task<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>> GetWarehouseShelvesWithStockInformationAsync(string? depoKodu = null, string? raf = null, CancellationToken cancellationToken = default);
Task<ApiResponse<List<WarehouseShelfStocksDto>>> GetWarehouseShelvesNestedAsync(string depoKodu, CancellationToken cancellationToken = default);
Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default);
Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default);
}
