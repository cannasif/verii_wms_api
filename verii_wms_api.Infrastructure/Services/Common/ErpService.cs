using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using WMS_WEBAPI.Models;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace WMS_WEBAPI.Services
{
    public class ErpService : IErpService
    {
        private readonly IErpUnitOfWork _erpUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly ICurrentUserService _executionContextAccessor;
        private readonly IRequestCancellationAccessor _requestCancellationAccessor;
        private readonly ILogger<ErpService> _logger;

        public ErpService(
            IErpUnitOfWork erpUnitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            ICurrentUserService executionContextAccessor,
            IRequestCancellationAccessor requestCancellationAccessor,
            ILogger<ErpService> logger)
        {
            _erpUnitOfWork = erpUnitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _executionContextAccessor = executionContextAccessor;
            _requestCancellationAccessor = requestCancellationAccessor;
            _logger = logger;
        }

        private CancellationToken ResolveCancellationToken(CancellationToken cancellationToken = default)
        {
            return _requestCancellationAccessor.Get(cancellationToken);
        }

        // OnHandQuantity işlemleri

        public async Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(int? depoKodu = null, string? stokKodu = null, string? seriNo = null, string? projeKodu = null, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var stokParam = string.IsNullOrWhiteSpace(stokKodu) ? null : stokKodu;
var seriParam = string.IsNullOrWhiteSpace(seriNo) ? null : seriNo;
var projeParam = string.IsNullOrWhiteSpace(projeKodu) ? null : projeKodu;

var rows = await _erpUnitOfWork.SqlQuery<RII_FN_ONHANDQUANTITY>("SELECT * FROM dbo.RII_FN_ONHANDQUANTITY({0}, {1}, {2}, {3})", depoKodu!, stokKodu!, seriNo!, projeKodu!)
.ToListAsync(cancellationToken);

var mappedList = _mapper.Map<List<OnHandQuantityDto>>(rows);

return ApiResponse<List<OnHandQuantityDto>>.SuccessResult(mappedList, _localizationService.GetLocalizedString("OnHandQuantityRetrievedSuccessfully"));
        }

        // Cari işlemleri
        public async Task<ApiResponse<List<CariDto>>> GetCarisAsync(string? cariKodu, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var subeFromContext = _executionContextAccessor.BranchCode;
var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

var result = await _erpUnitOfWork.SqlQuery<RII_VW_CARI>("SELECT * FROM dbo.RII_FN_CARI({0}, {1})", string.IsNullOrWhiteSpace(cariKodu) ? null! : cariKodu, subeKodu!)
    .ToListAsync(cancellationToken);

var mappedResult = _mapper.Map<List<CariDto>>(result);
return ApiResponse<List<CariDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<CariDto>>> GetCarisByCodesAsync(IEnumerable<string> cariKodlari, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var codes = (cariKodlari ?? Array.Empty<string>())
    .Where(s => !string.IsNullOrWhiteSpace(s))
    .Select(s => s.Trim())
    .Distinct()
    .ToList();

var cariParam = codes.Count == 0 ? null : string.Join(",", codes);

var subeFromContext = _executionContextAccessor.BranchCode;
var subeCsv = string.IsNullOrWhiteSpace(subeFromContext)
    ? null
    : string.Join(",", subeFromContext.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));

var result = await _erpUnitOfWork.SqlQuery<RII_VW_CARI>("SELECT * FROM dbo.RII_FN_CARI({0}, {1})", cariParam!, subeCsv!)
    .ToListAsync(cancellationToken);

var mappedResult = _mapper.Map<List<CariDto>>(result);
return ApiResponse<List<CariDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
        }

        // Stok işlemleri
        public async Task<ApiResponse<List<StokDto>>> GetStoksAsync(string? stokKodu, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var subeFromContext = _executionContextAccessor.BranchCode;
var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

var result = await _erpUnitOfWork.SqlQuery<RII_VW_STOK>("SELECT * FROM dbo.RII_FN_STOK({0}, {1})", string.IsNullOrWhiteSpace(stokKodu) ? null! : stokKodu, subeKodu!)
    .ToListAsync(cancellationToken);
var mappedResult = _mapper.Map<List<StokDto>>(result);

return ApiResponse<List<StokDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("StokRetrievedSuccessfully"));
        }

        

        

        public async Task<ApiResponse<IEnumerable<T>>> PopulateStockNamesAsync<T>(IEnumerable<T> dtos, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);

var codeProp = typeof(T).GetProperty("StockCode");
var nameProp = typeof(T).GetProperty("StockName");
var yapCodeProp = typeof(T).GetProperty("YapKod");
var yapNameProp = typeof(T).GetProperty("YapAcik");

var list = (dtos ?? Array.Empty<T>()).ToList();

var codes = codeProp == null
    ? new List<string>()
    : list
        .Select(d => codeProp.GetValue(d) as string)
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Select(s => s!.Trim())
        .Distinct()
        .ToList();

var stokParam = codes.Count == 0 ? null : string.Join(",", codes);

var yapKodlar = yapCodeProp == null
    ? new List<string>()
    : list
        .Select(d => yapCodeProp.GetValue(d) as string)
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Select(s => s!.Trim())
        .Distinct()
        .ToList();
var yapParam = yapKodlar.Count == 0 ? null : string.Join(",", yapKodlar);

var subeFromContext = _executionContextAccessor.BranchCode;
var subeCsv = string.IsNullOrWhiteSpace(subeFromContext)
    ? null
    : string.Join(",", subeFromContext.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));

var result = await _erpUnitOfWork.SqlQuery<RII_VW_STOK>("SELECT * FROM dbo.RII_FN_STOK({0}, {1})", stokParam!, subeCsv!)
    .ToListAsync(cancellationToken);

var data = _mapper.Map<List<StokDto>>(result);
var stockNameByCode = data
    .Where(s => !string.IsNullOrWhiteSpace(s.StokKodu))
    .GroupBy(s => s.StokKodu!.Trim(), StringComparer.OrdinalIgnoreCase)
    .ToDictionary(g => g.Key, g => g.First().StokAdi ?? string.Empty, StringComparer.OrdinalIgnoreCase);

var resultYapkod = await _erpUnitOfWork.SqlQuery<RII_FN_STOKYAPKOD>("SELECT * FROM dbo.RII_FN_STOKYAPKOD({0}, {1})", yapParam!, subeCsv!)
    .ToListAsync(cancellationToken);

var dataYapKod = _mapper.Map<List<StokYapKodDto>>(resultYapkod);

var yapNameByCode = dataYapKod
    .Where(c => !string.IsNullOrWhiteSpace(c.YapKod))
    .GroupBy(c => c.YapKod.Trim(), StringComparer.OrdinalIgnoreCase)
    .ToDictionary(g => g.Key, g => g.First().YapAcik ?? string.Empty, StringComparer.OrdinalIgnoreCase);

foreach (var dto in list)
{
    //Stok koduna göre adını doldur
    if (codeProp != null && nameProp != null)
    {
        var code = codeProp.GetValue(dto) as string;
        var trimmed = code?.Trim();
        if (!string.IsNullOrEmpty(trimmed) && stockNameByCode.TryGetValue(trimmed, out var nm))
        {
            nameProp.SetValue(dto, nm);
        }
    }
    //Yapılandırma Koduna göre adını doldur
    if (yapCodeProp != null && yapNameProp != null)
    {
        var yapCode = yapCodeProp.GetValue(dto) as string;
        var trimmedYap = yapCode?.Trim();
        if (!string.IsNullOrEmpty(trimmedYap) && yapNameByCode.TryGetValue(trimmedYap, out var yapNm))
        {
            yapNameProp.SetValue(dto, yapNm);
        }
    }
}

return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("StokRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<T>>> PopulateCustomerNamesAsync<T>(IEnumerable<T> dtos, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var list = (dtos ?? Array.Empty<T>()).ToList();
var codeProp = typeof(T).GetProperty("CustomerCode");
var nameProp = typeof(T).GetProperty("CustomerName");

var codes = codeProp == null
    ? new List<string>()
    : list
        .Select(d => codeProp.GetValue(d) as string)
        .Where(s => !string.IsNullOrWhiteSpace(s))
        .Select(s => s!.Trim())
        .Distinct()
        .ToList();

var cariParam = codes.Count == 0 ? null : string.Join(",", codes);

var subeFromContext = _executionContextAccessor.BranchCode;
var subeCsv = string.IsNullOrWhiteSpace(subeFromContext)
    ? null
    : string.Join(",", subeFromContext.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)));

var result = await _erpUnitOfWork.SqlQuery<RII_VW_CARI>("SELECT * FROM dbo.RII_FN_CARI({0}, {1})", cariParam!, subeCsv!)
    .ToListAsync(cancellationToken);

var data = _mapper.Map<List<CariDto>>(result);
var nameByCode = data
    .Where(c => !string.IsNullOrWhiteSpace(c.CariKod))
    .GroupBy(c => c.CariKod.Trim(), StringComparer.OrdinalIgnoreCase)
    .ToDictionary(g => g.Key, g => g.First().CariIsim ?? string.Empty, StringComparer.OrdinalIgnoreCase);

 
foreach (var dto in list)
{      
     if (codeProp != null && nameProp != null)
      {
        var code = codeProp.GetValue(dto) as string;
        var trimmed = code?.Trim();
        if (!string.IsNullOrEmpty(trimmed) && nameByCode.TryGetValue(trimmed, out var nm))
        {
            nameProp.SetValue(dto, nm);
        }
    }
}

return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
        }

        // Depo işlemleri
        public async Task<ApiResponse<List<DepoDto>>> GetDeposAsync(short? depoKodu, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var subeFromContext = _executionContextAccessor.BranchCode;
var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

var result = await _erpUnitOfWork.SqlQuery<RII_VW_DEPO>("SELECT * FROM dbo.RII_FN_DEPO({0}, {1})", depoKodu!, subeKodu!)
    .ToListAsync(cancellationToken);
var mappedResult = _mapper.Map<List<DepoDto>>(result);

return ApiResponse<List<DepoDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("DepoRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<IEnumerable<T>>> PopulateWarehouseNamesAsync<T>(IEnumerable<T> dtos, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var list = (dtos ?? Array.Empty<T>()).ToList();
var srcCodeProp = typeof(T).GetProperty("SourceWarehouse");
var srcNameProp = typeof(T).GetProperty("SourceWarehouseName");
var tgtCodeProp = typeof(T).GetProperty("TargetWarehouse");
var tgtNameProp = typeof(T).GetProperty("TargetWarehouseName");

var subeFromContext = _executionContextAccessor.BranchCode;
var subeKodu = string.IsNullOrWhiteSpace(subeFromContext) ? null : subeFromContext;

var rows = await _erpUnitOfWork.SqlQuery<RII_VW_DEPO>("SELECT * FROM dbo.RII_FN_DEPO({0}, {1})", null!, subeKodu!)
    .ToListAsync(cancellationToken);

var depos = _mapper.Map<List<DepoDto>>(rows);
var nameByCode = depos
    .GroupBy(d => d.DepoKodu.ToString().Trim(), StringComparer.OrdinalIgnoreCase)
    .ToDictionary(g => g.Key, g => g.First().DepoIsmi ?? string.Empty, StringComparer.OrdinalIgnoreCase);

foreach (var dto in list)
{
    // SourceWarehouse/SourceWarehouseName
    if (srcCodeProp != null && srcNameProp != null)
    {
        var codeObj = srcCodeProp.GetValue(dto);
        var codeStr = codeObj == null ? null : codeObj.ToString()?.Trim();
        if (!string.IsNullOrEmpty(codeStr) && nameByCode.TryGetValue(codeStr!, out var nm))
        {
            srcNameProp.SetValue(dto, nm);
        }
    }

    // TargetWarehouse/TargetWarehouseName
    if (tgtCodeProp != null && tgtNameProp != null)
    {
        var codeObj = tgtCodeProp.GetValue(dto);
        var codeStr = codeObj == null ? null : codeObj.ToString()?.Trim();
        if (!string.IsNullOrEmpty(codeStr) && nameByCode.TryGetValue(codeStr!, out var nm))
        {
            tgtNameProp.SetValue(dto, nm);
        }
    }
}

return ApiResponse<IEnumerable<T>>.SuccessResult(list, _localizationService.GetLocalizedString("DepoRetrievedSuccessfully"));
        }

        // Proje işlemleri
        public async Task<ApiResponse<List<ProjeDto>>> GetProjelerAsync(CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var result = await _erpUnitOfWork.Query<RII_VW_PROJE>().ToListAsync(cancellationToken);
var mappedResult = _mapper.Map<List<ProjeDto>>(result);

return ApiResponse<List<ProjeDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProjeRetrievedSuccessfully"));
        }
        public async Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(string bar, int depoKodu, int modul, int kullaniciId, string barkodGrubu, int hareketTuru, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var rows = await _erpUnitOfWork.SqlQuery<RII_FN_STOKBARCODE>("SELECT * FROM dbo.RII_FN_STOKBARCODE({0}, {1}, {2}, {3}, {4}, {5})", bar, depoKodu, modul, kullaniciId, barkodGrubu, hareketTuru)
    .ToListAsync(cancellationToken);

var mappedList = _mapper.Map<List<StokBarcodeDto>>(rows);
return ApiResponse<List<StokBarcodeDto>>.SuccessResult(mappedList, _localizationService.GetLocalizedString("StokBarcodeRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var connectionString = _erpUnitOfWork.GetConnectionString();
if (string.IsNullOrWhiteSpace(connectionString))
{
    _logger.LogWarning("GetBranchesAsync called but ErpConnection is not configured.");
    return ApiResponse<List<BranchDto>>.SuccessResult(
        new List<BranchDto>(),
        _localizationService.GetLocalizedString("BranchesRetrievedSuccessfully"));
}

_logger.LogInformation(
    "ERP branch list requested. BranchNo: {BranchNo}, ConnectionStringPresent: {HasConnectionString}",
    branchNo,
    !string.IsNullOrWhiteSpace(connectionString));

var rows = await _erpUnitOfWork.SqlQuery<RII_FN_BRANCHES>("SELECT * FROM dbo.RII_FN_BRANCHES({0})", branchNo.HasValue ? branchNo.Value : DBNull.Value)
    .ToListAsync(cancellationToken);

_logger.LogInformation("ERP branch list retrieved successfully. Count: {Count}", rows.Count);

var mappedList = _mapper.Map<List<BranchDto>>(rows);
return ApiResponse<List<BranchDto>>.SuccessResult(mappedList, _localizationService.GetLocalizedString("BranchesRetrievedSuccessfully"));
        }

        // Depo ve Raf işlemleri
        public async Task<ApiResponse<List<WarehouseAndShelvesDto>>> GetWarehouseAndShelvesAsync(string? depoKodu = null, string? raf = null, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var result = await _erpUnitOfWork.SqlQuery<RII_FN_WAREHOUSE_SHELF>("SELECT * FROM dbo.RII_FN_WAREHOUSE_SHELF({0}, {1})", depoKodu!, raf!)
    .ToListAsync(cancellationToken);

var mappedResult = _mapper.Map<List<WarehouseAndShelvesDto>>(result);

return ApiResponse<List<WarehouseAndShelvesDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("WarehouseAndShelvesRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>> GetWarehouseShelvesWithStockInformationAsync(string? depoKodu = null, string? raf = null, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var result = await _erpUnitOfWork.SqlQuery<RII_FN_STOCK_WAREHOUSE>("SELECT * FROM dbo.RII_FN_STOCK_WAREHOUSE({0}, {1})", depoKodu!, raf!)
    .ToListAsync(cancellationToken);

var mappedResult = _mapper.Map<List<WarehouseShelvesWithStockInformationDto>>(result);

return ApiResponse<List<WarehouseShelvesWithStockInformationDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("StockWarehouseRetrievedSuccessfully"));
        }

        // Üretim Emri işlemleri
        public async Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var result = await _erpUnitOfWork.SqlQuery<RII_FN_PRODUCT_HEADER>("SELECT * FROM dbo.RII_FN_PRODUCT_HEADER({0})", isemriNo)
    .ToListAsync(cancellationToken);

var mappedResult = _mapper.Map<List<ProductHeaderDto>>(result);

return ApiResponse<List<ProductHeaderDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProductHeaderRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var result = await _erpUnitOfWork.SqlQuery<RII_FN_PRODUCT_LINE>("SELECT * FROM dbo.RII_FN_PRODUCT_LINE({0}, {1}, {2})", isemriNo!, fisNo!, mamulKodu!)
    .ToListAsync(cancellationToken);

var mappedResult = _mapper.Map<List<ProductLineDto>>(result);

return ApiResponse<List<ProductLineDto>>.SuccessResult(mappedResult, _localizationService.GetLocalizedString("ProductLinesRetrievedSuccessfully"));
        }

        public async Task<ApiResponse<List<WarehouseShelfStocksDto>>> GetWarehouseShelvesNestedAsync(string depoKodu, CancellationToken cancellationToken = default)
        {
cancellationToken = ResolveCancellationToken(cancellationToken);
var shelvesRows = await _erpUnitOfWork.SqlQuery<RII_FN_WAREHOUSE_SHELF>("SELECT * FROM dbo.RII_FN_WAREHOUSE_SHELF({0}, {1})", depoKodu, null!)
    .ToListAsync(cancellationToken);
var shelves = _mapper.Map<List<WarehouseAndShelvesDto>>(shelvesRows);

var stocksRows = await _erpUnitOfWork.SqlQuery<RII_FN_STOCK_WAREHOUSE>("SELECT * FROM dbo.RII_FN_STOCK_WAREHOUSE({0}, {1})", depoKodu, null!)
    .ToListAsync(cancellationToken);

var stocksByShelf = stocksRows
    .Where(r => !string.IsNullOrWhiteSpace(r.HUCRE_KODU))
    .GroupBy(r => r.HUCRE_KODU!.Trim(), StringComparer.OrdinalIgnoreCase)
    .ToDictionary(g => g.Key, g => _mapper.Map<List<ShelfStockDto>>(g.ToList()), StringComparer.OrdinalIgnoreCase);

var result = shelves
    .OrderBy(s => s.HucreKodu)
    .Select(s => new WarehouseShelfStocksDto
    {
        DepoKodu = s.DepoKodu,
        HucreKodu = s.HucreKodu,
        Stoklar = s.HucreKodu != null && stocksByShelf.TryGetValue(s.HucreKodu.Trim(), out var list) ? list : new List<ShelfStockDto>()
    })
    .ToList();

return ApiResponse<List<WarehouseShelfStocksDto>>.SuccessResult(result, _localizationService.GetLocalizedString("WarehouseShelvesNestedRetrievedSuccessfully"));
        }

    }
}
