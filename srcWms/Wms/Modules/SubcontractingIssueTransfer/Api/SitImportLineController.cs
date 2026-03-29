using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingIssueTransfer.Dtos;
using Wms.Application.SubcontractingIssueTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingIssueTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SitImportLineController : ControllerBase
{
    private readonly ISitImportLineService _service;

    public SitImportLineController(ISitImportLineService service)
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
    public async Task<ActionResult<ApiResponse<SitImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-header/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SitImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("by-line/{lineId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SitImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByLineIdAsync(lineId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("warehouseSubcontractingIssueTransferOrderCollectedBarcodes/{headerId:long}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<SitImportLineWithRoutesDto>>>> SubcontractingIssueTransferOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SitImportLineDto>>> Create([FromBody] CreateSitImportLineDto createDto, CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createDto, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<ApiResponse<SitImportLineDto>>> Update(long id, [FromBody] UpdateSitImportLineDto updateDto, CancellationToken cancellationToken = default)
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
    public async Task<ActionResult<ApiResponse<PagedResponse<SitImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("addBarcodeBasedonAssignedOrder")]
    public async Task<ActionResult<ApiResponse<SitImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddSitImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _service.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
