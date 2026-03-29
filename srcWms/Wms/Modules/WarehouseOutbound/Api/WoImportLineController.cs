using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Application.WarehouseOutbound.Services;

namespace Wms.WebApi.Controllers.WarehouseOutbound;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class WoImportLineController : ControllerBase
{
    private readonly IWoImportLineService _service;

    public WoImportLineController(IWoImportLineService service)
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
    public async Task<ActionResult<ApiResponse<WoImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WoImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-line/{lineId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WoImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("warehouseOutboundOrderCollectedBarcodes/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WoImportLineWithRoutesDto>>>> WarehouseOutboundOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WoImportLineDto>>> Create([FromBody] CreateWoImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<WoImportLineDto>>> Update(long id, [FromBody] UpdateWoImportLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<WoImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("addBarcodeBasedonAssignedOrder")]
    public async Task<ActionResult<ApiResponse<WoImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddWoImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
