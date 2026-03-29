using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.ProductionTransfer.Dtos;
using Wms.Application.ProductionTransfer.Services;

namespace Wms.WebApi.Controllers.ProductionTransfer;

[ApiController]
[Route("api/[controller]")]
public sealed class PtFunctionController : ControllerBase
{
    private readonly IPtFunctionService _service;

    public PtFunctionController(IPtFunctionService service)
    {
        _service = service;
    }

    [HttpGet("getProductionTransferHeader")]
    public async Task<ActionResult<ApiResponse<List<ProductHeaderDto>>>> GetProductHeader([FromQuery] string isemriNo, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProductHeaderAsync(isemriNo, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("getProductionTransferLines")]
    public async Task<ActionResult<ApiResponse<List<ProductLineDto>>>> GetProductLines([FromQuery] string? isemriNo = null, [FromQuery] string? fisNo = null, [FromQuery] string? mamulKodu = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProductLinesAsync(isemriNo, fisNo, mamulKodu, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
