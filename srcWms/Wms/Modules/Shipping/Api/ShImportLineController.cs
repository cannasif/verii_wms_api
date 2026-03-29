using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Shipping.Dtos;
using Wms.Application.Shipping.Services;

namespace Wms.WebApi.Controllers.Shipping;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ShImportLineController : ControllerBase
{
    private readonly IShImportLineService _service;

    public ShImportLineController(IShImportLineService service)
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
    public async Task<ActionResult<ApiResponse<ShImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ShImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-line/{lineId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ShImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("warehouseShipmentOrderCollectedBarcodes/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ShImportLineWithRoutesDto>>>> ShippingOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ShImportLineDto>>> Create([FromBody] CreateShImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<ShImportLineDto>>> Update(long id, [FromBody] UpdateShImportLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<ShImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("addBarcodeBasedonAssignedOrder")]
    public async Task<ActionResult<ApiResponse<ShImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddShImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
