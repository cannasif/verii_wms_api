using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Application.WarehouseTransfer.Services;

namespace Wms.WebApi.Controllers.WarehouseTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class WtImportLineController : ControllerBase
{
    private readonly IWtImportLineService _service;

    public WtImportLineController(IWtImportLineService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponse<WtImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-line/{lineId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-route/{routeId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineDto>>>> GetByRouteId(long routeId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByRouteIdAsync(routeId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("warehouseTransferOrderCollectedBarcodes/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>>> WarehouseTransferOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WtImportLineDto>>> Create([FromBody] CreateWtImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<WtImportLineDto>>> Update(long id, [FromBody] UpdateWtImportLineDto updateDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("paged")]
    public async Task<ActionResult<ApiResponse<PagedResponse<WtImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("addBarcodeBasedonAssignedOrder")]
    public async Task<ActionResult<ApiResponse<WtImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddWtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
