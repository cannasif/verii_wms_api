using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.InventoryCount.Dtos;
using Wms.Application.InventoryCount.Services;

namespace Wms.WebApi.Controllers.InventoryCount;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class IcImportLineController : ControllerBase
{
    private readonly IIcImportLineService _service;
    public IcImportLineController(IIcImportLineService service) => _service = service;
    [HttpGet] public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost("paged")] public async Task<ActionResult<ApiResponse<PagedResponse<IcImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default) { var r = await _service.GetPagedAsync(request, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("{id:long}")] public async Task<ActionResult<ApiResponse<IcImportLineDto>>> GetById(long id, CancellationToken cancellationToken = default) { var r = await _service.GetByIdAsync(id, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("header/{headerId:long}")] public async Task<ActionResult<ApiResponse<IEnumerable<IcImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default) { var r = await _service.GetByHeaderIdAsync(headerId, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpGet("inventoryCountOrderCollectedBarcodes/{headerId:long}")] public async Task<ActionResult<ApiResponse<IEnumerable<IcImportLineWithRoutesDto>>>> InventoryCountOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default) { var r = await _service.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPost] public async Task<ActionResult<ApiResponse<IcImportLineDto>>> Create([FromBody] CreateIcImportLineDto createDto, CancellationToken cancellationToken = default) { var r = await _service.CreateAsync(createDto, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpPut("{id:long}")] public async Task<ActionResult<ApiResponse<IcImportLineDto>>> Update(long id, [FromBody] UpdateIcImportLineDto updateDto, CancellationToken cancellationToken = default) { var r = await _service.UpdateAsync(id, updateDto, cancellationToken); return StatusCode(r.StatusCode, r); }
    [HttpDelete("{id:long}")] public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default) { var r = await _service.SoftDeleteAsync(id, cancellationToken); return StatusCode(r.StatusCode, r); }
}
