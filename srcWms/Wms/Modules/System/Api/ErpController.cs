using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Application.System.Dtos;
using Wms.Application.System.Services;

namespace Wms.WebApi.Controllers.System;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ErpController : ControllerBase
{
    private readonly IErpService _service; public ErpController(IErpService service) => _service = service;
    [HttpGet("getOnHandQuantities")] public async Task<ActionResult<ApiResponse<List<OnHandQuantityDto>>>> GetOnHandQuantities([FromQuery] int? depoKodu = null, [FromQuery] string? stokKodu = null, [FromQuery] string? seriNo = null, [FromQuery] string? projeKodu = null, CancellationToken cancellationToken = default) { var r = await _service.GetOnHandQuantitiesAsync(depoKodu, stokKodu, seriNo, projeKodu, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("customers/paged")] public async Task<ActionResult<ApiResponse<PagedResponse<CariDto>>>> GetCarisPaged([FromBody] PagedRequest request, [FromQuery] string? cariKodu = null, CancellationToken cancellationToken = default) { var r = await _service.GetCarisPagedAsync(request, cariKodu, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("products/paged")] public async Task<ActionResult<ApiResponse<PagedResponse<StokDto>>>> GetStoksPaged([FromBody] PagedRequest request, [FromQuery] string? stokKodu = null, CancellationToken cancellationToken = default) { var r = await _service.GetStoksPagedAsync(request, stokKodu, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("warehouses/paged")] public async Task<ActionResult<ApiResponse<PagedResponse<DepoDto>>>> GetDeposPaged([FromBody] PagedRequest request, [FromQuery] short? depoKodu = null, CancellationToken cancellationToken = default) { var r = await _service.GetDeposPagedAsync(request, depoKodu, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("projects/paged")] public async Task<ActionResult<ApiResponse<PagedResponse<ProjeDto>>>> GetProjelerPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetProjelerPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getStokBarcode")] public async Task<ActionResult<ApiResponse<List<StokBarcodeDto>>>> GetStokBarcode([FromQuery] string bar, [FromQuery] int depoKodu, [FromQuery] int modul, [FromQuery] int kullaniciId, [FromQuery] string barkodGrubu, [FromQuery] int hareketTuru, CancellationToken cancellationToken = default) { var r = await _service.GetStokBarcodeAsync(bar, depoKodu, modul, kullaniciId, barkodGrubu, hareketTuru, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getBranches")][AllowAnonymous] public async Task<ActionResult<ApiResponse<List<BranchDto>>>> GetBranches([FromQuery] int? branchNo = null, CancellationToken cancellationToken = default) { var r = await _service.GetBranchesAsync(branchNo, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getWarehouseAndShelves")] public async Task<ActionResult<ApiResponse<List<WarehouseAndShelvesDto>>>> GetWarehouseAndShelves([FromQuery] string? depoKodu = null, [FromQuery] string? raf = null, CancellationToken cancellationToken = default) { var r = await _service.GetWarehouseAndShelvesAsync(depoKodu, raf, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getWarehouseShelvesWithStockInformation")] public async Task<ActionResult<ApiResponse<List<WarehouseShelvesWithStockInformationDto>>>> GetWarehouseShelvesWithStockInformation([FromQuery] string? depoKodu = null, [FromQuery] string? raf = null, CancellationToken cancellationToken = default) { var r = await _service.GetWarehouseShelvesWithStockInformationAsync(depoKodu, raf, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getWarehouseShelvesNested")] public async Task<ActionResult<ApiResponse<List<WarehouseShelfStocksDto>>>> GetWarehouseShelvesNested([FromQuery] string depoKodu, CancellationToken cancellationToken = default) { var r = await _service.GetWarehouseShelvesNestedAsync(depoKodu, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getProductionHeader")] public async Task<ActionResult<ApiResponse<List<ProductHeaderDto>>>> GetProductHeader([FromQuery] string isemriNo, CancellationToken cancellationToken = default) { var r = await _service.GetProductHeaderAsync(isemriNo, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("getProductionLines")] public async Task<ActionResult<ApiResponse<List<ProductLineDto>>>> GetProductLines([FromQuery] string? isemriNo = null, [FromQuery] string? fisNo = null, [FromQuery] string? mamulKodu = null, CancellationToken cancellationToken = default) { var r = await _service.GetProductLinesAsync(isemriNo, fisNo, mamulKodu, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("health-check")][AllowAnonymous] public ActionResult<ApiResponse<object>> HealthCheck() => Ok(ApiResponse<object>.SuccessResult(new { Status = "Healthy", Timestamp = DateTime.UtcNow }, "Health check completed."));
}
