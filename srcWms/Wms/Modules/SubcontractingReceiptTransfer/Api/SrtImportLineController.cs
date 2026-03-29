using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Application.SubcontractingReceiptTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingReceiptTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SrtImportLineController : ControllerBase
{
    private readonly ISrtImportLineService _service;

    public SrtImportLineController(ISrtImportLineService service)
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
    public async Task<ActionResult<ApiResponse<SrtImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-line/{lineId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("warehouseSubcontractingReceiptTransferOrderCollectedBarcodes/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SrtImportLineWithRoutesDto>>>> SubcontractingReceiptTransferOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> Create([FromBody] CreateSrtImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> Update(long id, [FromBody] UpdateSrtImportLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<SrtImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("addBarcodeBasedonAssignedOrder")]
    public async Task<ActionResult<ApiResponse<SrtImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddSrtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
