using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Application.WarehouseTransfer.Services;

namespace Wms.WebApi.Controllers.WarehouseTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class WtFunctionController : ControllerBase
{
    private readonly IWtFunctionService _service;

    public WtFunctionController(IWtFunctionService service)
    {
        _service = service;
    }

    [HttpGet("headers/{customerCode}")]
    public async Task<ActionResult<ApiResponse<List<TransferOpenOrderHeaderDto>>>> GetTransferOpenOrderHeader(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetTransferOpenOrderHeaderAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/{siparisNoCsv}")]
    public async Task<ActionResult<ApiResponse<List<TransferOpenOrderLineDto>>>> GetTransferOpenOrderLine(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetTransferOpenOrderLineAsync(siparisNoCsv, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
