using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.Production.Dtos;
using Wms.Application.Production.Services;

namespace Wms.WebApi.Controllers.Production;

[ApiController]
[Route("api/[controller]")]
public sealed class PrFunctionController : ControllerBase
{
    private readonly IPrFunctionService _service;

    public PrFunctionController(IPrFunctionService service)
    {
        _service = service;
    }

    [HttpGet("getProductionHeader")]
    public async Task<ActionResult<ApiResponse<List<ProductHeaderDto>>>> GetProductHeader([FromQuery] string isemriNo, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProductHeaderAsync(isemriNo, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("getProductionLines")]
    public async Task<ActionResult<ApiResponse<List<ProductLineDto>>>> GetProductLines([FromQuery] string? isemriNo = null, [FromQuery] string? fisNo = null, [FromQuery] string? mamulKodu = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetProductLinesAsync(isemriNo, fisNo, mamulKodu, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
