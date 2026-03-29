using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Application.Production.Services;
using Wms.Application.System.Dtos;
using Wms.Application.System.Services;

namespace Wms.Infrastructure.Services.Erp;

public sealed class PragmaticErpService : IErpService
{
    private readonly IPrFunctionService _prFunctionService;
    private readonly IConfiguration _configuration;
    private readonly ILocalizationService _localizationService;
    private readonly ILogger<PragmaticErpService> _logger;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public PragmaticErpService(
        IPrFunctionService prFunctionService,
        IConfiguration configuration,
        ILocalizationService localizationService,
        ILogger<PragmaticErpService> logger,
        ICurrentUserAccessor currentUserAccessor)
    {
        _prFunctionService = prFunctionService;
        _configuration = configuration;
        _localizationService = localizationService;
        _logger = logger;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<ApiResponse<List<OnHandQuantityDto>>> GetOnHandQuantitiesAsync(
        int? depoKodu = null,
        string? stokKodu = null,
        string? seriNo = null,
        string? projeKodu = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_ONHANDQUANTITY(@depoKodu, @stokKodu, @seriNo, @projeKodu)",
                reader => new OnHandQuantityDto
                {
                    DepoKodu = GetValue<int>(reader, "DEPO_KODU"),
                    StokKodu = GetNullableString(reader, "STOK_KODU"),
                    ProjeKodu = GetNullableString(reader, "PROJE_KODU"),
                    SeriNo = GetNullableString(reader, "SERI_NO"),
                    HucreKodu = GetNullableString(reader, "HUCRE_KODU"),
                    Kaynak = GetNullableString(reader, "KAYNAK"),
                    Bakiye = GetNullableDecimal(reader, "BAKIYE"),
                    StokAdi = GetNullableString(reader, "STOK_ADI"),
                    DepoIsmi = GetNullableString(reader, "DEPO_ISMI")
                },
                cancellationToken,
                new SqlParameter("@depoKodu", ToDbValue(depoKodu)),
                new SqlParameter("@stokKodu", ToDbValue(stokKodu)),
                new SqlParameter("@seriNo", ToDbValue(seriNo)),
                new SqlParameter("@projeKodu", ToDbValue(projeKodu)));

            return ApiResponse<List<OnHandQuantityDto>>.SuccessResult(
                rows,
                _localizationService.GetLocalizedString("OnHandQuantityRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<List<OnHandQuantityDto>>(ex, "OnHandQuantityRetrievalError");
        }
    }

    public async Task<ApiResponse<PagedResponse<CariDto>>> GetCarisPagedAsync(
        PagedRequest request,
        string? cariKodu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var effectiveCariKodu = FirstNonEmpty(cariKodu, request?.Search);
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_CARI(@cariKodu, @branchCode)",
                reader => new CariDto
                {
                    SubeKodu = GetValue<short>(reader, "SUBE_KODU"),
                    IsletmeKodu = GetValue<short>(reader, "ISLETME_KODU"),
                    CariKod = GetStringOrEmpty(reader, "CARI_KOD"),
                    CariIsim = GetNullableString(reader, "CARI_ISIM")
                },
                cancellationToken,
                new SqlParameter("@cariKodu", ToDbValue(effectiveCariKodu)),
                new SqlParameter("@branchCode", ToDbValue(GetBranchCodeOrNull())));

            return ApiResponse<PagedResponse<CariDto>>.SuccessResult(
                ToPagedResponse(rows, request),
                _localizationService.GetLocalizedString("CariRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<PagedResponse<CariDto>>(ex, "CariRetrievalError");
        }
    }

    public async Task<ApiResponse<PagedResponse<StokDto>>> GetStoksPagedAsync(
        PagedRequest request,
        string? stokKodu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var effectiveStokKodu = FirstNonEmpty(stokKodu, request?.Search);
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_STOK(@stokKodu, @branchCode)",
                reader => new StokDto
                {
                    SubeKodu = GetValue<short>(reader, "SUBE_KODU"),
                    IsletmeKodu = GetValue<short>(reader, "ISLETME_KODU"),
                    StokKodu = GetStringOrEmpty(reader, "STOK_KODU"),
                    UreticiKodu = GetStringOrEmpty(reader, "URETICI_KODU"),
                    StokAdi = GetStringOrEmpty(reader, "STOK_ADI"),
                    GrupKodu = GetStringOrEmpty(reader, "GRUP_KODU"),
                    Kod1 = GetStringOrEmpty(reader, "KOD_1"),
                    Kod2 = GetStringOrEmpty(reader, "KOD_2"),
                    Kod3 = GetStringOrEmpty(reader, "KOD_3"),
                    Kod4 = GetStringOrEmpty(reader, "KOD_4"),
                    Kod5 = GetStringOrEmpty(reader, "KOD_5")
                },
                cancellationToken,
                new SqlParameter("@stokKodu", ToDbValue(effectiveStokKodu)),
                new SqlParameter("@branchCode", ToDbValue(GetBranchCodeOrNull())));

            return ApiResponse<PagedResponse<StokDto>>.SuccessResult(
                ToPagedResponse(rows, request),
                _localizationService.GetLocalizedString("StokRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<PagedResponse<StokDto>>(ex, "StokRetrievalError");
        }
    }

    public async Task<ApiResponse<PagedResponse<DepoDto>>> GetDeposPagedAsync(
        PagedRequest request,
        short? depoKodu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            short? effectiveDepoKodu = depoKodu;
            if (!effectiveDepoKodu.HasValue && short.TryParse(request?.Search, out var parsedDepoKodu))
            {
                effectiveDepoKodu = parsedDepoKodu;
            }

            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_DEPO(@depoKodu, @branchCode)",
                reader => new DepoDto
                {
                    DepoKodu = GetValue<short>(reader, "DEPO_KODU"),
                    DepoIsmi = GetStringOrEmpty(reader, "DEPO_ISMI")
                },
                cancellationToken,
                new SqlParameter("@depoKodu", ToDbValue(effectiveDepoKodu)),
                new SqlParameter("@branchCode", ToDbValue(GetBranchCodeOrNull())));

            return ApiResponse<PagedResponse<DepoDto>>.SuccessResult(
                ToPagedResponse(rows, request),
                _localizationService.GetLocalizedString("DepoRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<PagedResponse<DepoDto>>(ex, "DepoRetrievalError");
        }
    }

    public async Task<ApiResponse<PagedResponse<ProjeDto>>> GetProjelerPagedAsync(
        PagedRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_PROJECTCODE()",
                reader => new ProjeDto
                {
                    ProjeKod = GetStringOrEmpty(reader, "PROJE_KODU"),
                    ProjeAciklama = GetStringOrEmpty(reader, "PROJE_ACIKLAMA")
                },
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(request?.Search))
            {
                rows = rows
                    .Where(x =>
                        x.ProjeKod.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                        x.ProjeAciklama.Contains(request.Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return ApiResponse<PagedResponse<ProjeDto>>.SuccessResult(
                ToPagedResponse(rows, request),
                _localizationService.GetLocalizedString("ProjeRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<PagedResponse<ProjeDto>>(ex, "ProjeRetrievalError");
        }
    }

    public async Task<ApiResponse<List<StokBarcodeDto>>> GetStokBarcodeAsync(
        string bar,
        int depoKodu,
        int modul,
        int kullaniciId,
        string barkodGrubu,
        int hareketTuru,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_STOKBARCODE(@bar, @depoKodu, @modul, @kullaniciId, @barkodGrubu, @hareketTuru)",
                reader => new StokBarcodeDto
                {
                    Barkod = GetNullableString(reader, "BARKOD"),
                    StokKodu = GetNullableString(reader, "STOK_KODU"),
                    StokAdi = GetNullableString(reader, "STOK_ADI"),
                    DepoKodu = GetNullableString(reader, "DEPO_KODU"),
                    DepoAdi = GetNullableString(reader, "DEPO_ADI"),
                    RafKodu = GetNullableString(reader, "RAF_KODU"),
                    YapKod = GetNullableString(reader, "YAPKOD"),
                    YapAcik = GetNullableString(reader, "YAPACIK"),
                    SeriBarkodMu = GetNullableBoolean(reader, "SERI_BARKODUMU"),
                    IsemriNo = GetNullableString(reader, "ISEMRI_NO")
                },
                cancellationToken,
                new SqlParameter("@bar", ToDbValue(bar)),
                new SqlParameter("@depoKodu", depoKodu),
                new SqlParameter("@modul", modul),
                new SqlParameter("@kullaniciId", kullaniciId),
                new SqlParameter("@barkodGrubu", ToDbValue(barkodGrubu)),
                new SqlParameter("@hareketTuru", hareketTuru));

            return ApiResponse<List<StokBarcodeDto>>.SuccessResult(
                rows,
                _localizationService.GetLocalizedString("StokBarcodeRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<List<StokBarcodeDto>>(ex, "StokBarcodeRetrievalError");
        }
    }

    public async Task<ApiResponse<List<BranchDto>>> GetBranchesAsync(int? branchNo = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_BRANCHES(@branchNo)",
                reader => new BranchDto
                {
                    SubeKodu = GetValue<short>(reader, "SUBE_KODU"),
                    Unvan = GetNullableString(reader, "UNVAN")
                },
                cancellationToken,
                new SqlParameter("@branchNo", ToDbValue(branchNo)));

            return ApiResponse<List<BranchDto>>.SuccessResult(
                rows,
                _localizationService.GetLocalizedString("BranchesRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ERP branch list retrieval failed. BranchNo: {BranchNo}", branchNo);
            return ApiResponse<List<BranchDto>>.ErrorResult(
                _localizationService.GetLocalizedString("InternalServerError"),
                _localizationService.GetLocalizedString("BranchesRetrievalError", ex.Message),
                StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<ApiResponse<List<WarehouseAndShelvesDto>>> GetWarehouseAndShelvesAsync(
        string? depoKodu = null,
        string? raf = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_WAREHOUSE_SHELF(@depoKodu, @raf)",
                reader => new WarehouseAndShelvesDto
                {
                    DepoKodu = GetNullableString(reader, "DEPO_KODU"),
                    HucreKodu = GetNullableString(reader, "HUCRE_KODU")
                },
                cancellationToken,
                new SqlParameter("@depoKodu", ToDbValue(depoKodu)),
                new SqlParameter("@raf", ToDbValue(raf)));

            return ApiResponse<List<WarehouseAndShelvesDto>>.SuccessResult(
                rows,
                _localizationService.GetLocalizedString("WarehouseAndShelvesRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<List<WarehouseAndShelvesDto>>(ex, "WarehouseAndShelvesRetrievalError");
        }
    }

    public async Task<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>> GetWarehouseShelvesWithStockInformationAsync(
        string? depoKodu = null,
        string? raf = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_STOCK_WAREHOUSE(@depoKodu, @raf)",
                reader => new WarehouseShelvesWithStockInformationDto
                {
                    DepoKodu = GetNullableString(reader, "DEPO_KODU"),
                    HucreKodu = GetNullableString(reader, "HUCRE_KODU"),
                    StokKodu = GetNullableString(reader, "STOK_KODU"),
                    StokAdi = GetNullableString(reader, "STOK_ADI"),
                    YapKod = GetNullableString(reader, "YAPKOD"),
                    YapAcik = GetNullableString(reader, "YAPACIK"),
                    SeriNo = GetNullableString(reader, "SERI_NO"),
                    Bakiye = GetNullableDecimal(reader, "BAKIYE")
                },
                cancellationToken,
                new SqlParameter("@depoKodu", ToDbValue(depoKodu)),
                new SqlParameter("@raf", ToDbValue(raf)));

            return ApiResponse<List<WarehouseShelvesWithStockInformationDto>>.SuccessResult(
                rows,
                _localizationService.GetLocalizedString("StockWarehouseRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<List<WarehouseShelvesWithStockInformationDto>>(ex, "StockWarehouseRetrievalError");
        }
    }

    public async Task<ApiResponse<List<WarehouseShelfStocksDto>>> GetWarehouseShelvesNestedAsync(
        string depoKodu,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var rows = await QueryAsync(
                "SELECT * FROM dbo.RII_FN_STOCK_WAREHOUSE(@depoKodu, @raf)",
                reader => new WarehouseShelfStocksDto
                {
                    DepoKodu = GetNullableString(reader, "DEPO_KODU"),
                    HucreKodu = GetNullableString(reader, "HUCRE_KODU"),
                    StokKodu = GetNullableString(reader, "STOK_KODU"),
                    StokAdi = GetNullableString(reader, "STOK_ADI"),
                    SeriNo = GetNullableString(reader, "SERI_NO"),
                    Bakiye = GetNullableDecimal(reader, "BAKIYE")
                },
                cancellationToken,
                new SqlParameter("@depoKodu", ToDbValue(depoKodu)),
                new SqlParameter("@raf", DBNull.Value));

            return ApiResponse<List<WarehouseShelfStocksDto>>.SuccessResult(
                rows,
                _localizationService.GetLocalizedString("WarehouseShelvesNestedRetrievedSuccessfully"));
        }
        catch (Exception ex)
        {
            return BuildError<List<WarehouseShelfStocksDto>>(ex, "WarehouseShelvesNestedRetrievalError");
        }
    }

    public Task<ApiResponse<List<ProductHeaderDto>>> GetProductHeaderAsync(string isemriNo, CancellationToken cancellationToken = default)
        => _prFunctionService.GetProductHeaderAsync(isemriNo, cancellationToken);

    public Task<ApiResponse<List<ProductLineDto>>> GetProductLinesAsync(string? isemriNo = null, string? fisNo = null, string? mamulKodu = null, CancellationToken cancellationToken = default)
        => _prFunctionService.GetProductLinesAsync(isemriNo, fisNo, mamulKodu, cancellationToken);

    private async Task<List<T>> QueryAsync<T>(
        string sql,
        Func<SqlDataReader, T> map,
        CancellationToken cancellationToken,
        params SqlParameter[] parameters)
    {
        var connectionString = _configuration.GetConnectionString("ErpConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ErpConnection is not configured.");
        }

        var result = new List<T>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(map(reader));
        }

        return result;
    }

    private ApiResponse<T> BuildError<T>(Exception ex, string messageKey)
    {
        _logger.LogError(ex, "ERP read operation failed for key {MessageKey}", messageKey);
        return ApiResponse<T>.ErrorResult(
            _localizationService.GetLocalizedString("InternalServerError"),
            _localizationService.GetLocalizedString(messageKey, ex.Message),
            StatusCodes.Status500InternalServerError);
    }

    private static PagedResponse<T> ToPagedResponse<T>(List<T> rows, PagedRequest? request)
    {
        var pageNumber = request?.PageNumber > 0 ? request.PageNumber : 1;
        var pageSize = request?.PageSize > 0 ? request.PageSize : 20;

        var pagedRows = rows
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResponse<T>(pagedRows, rows.Count, pageNumber, pageSize);
    }

    private string? GetBranchCodeOrNull()
    {
        var branchCode = _currentUserAccessor.BranchCode;
        return string.IsNullOrWhiteSpace(branchCode) ? null : branchCode.Trim();
    }

    private static object ToDbValue<T>(T? value)
    {
        if (value is null)
        {
            return DBNull.Value;
        }

        if (value is string stringValue)
        {
            return string.IsNullOrWhiteSpace(stringValue) ? DBNull.Value : stringValue.Trim();
        }

        return value;
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim();
    }

    private static string GetStringOrEmpty(SqlDataReader reader, string columnName)
        => GetNullableString(reader, columnName) ?? string.Empty;

    private static string? GetNullableString(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    private static decimal? GetNullableDecimal(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetFieldValue<decimal>(ordinal);
    }

    private static bool? GetNullableBoolean(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);
    }

    private static T GetValue<T>(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? default! : reader.GetFieldValue<T>(ordinal);
    }
}
