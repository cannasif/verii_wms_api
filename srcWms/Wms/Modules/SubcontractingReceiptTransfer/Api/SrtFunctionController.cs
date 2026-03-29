using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Application.SubcontractingReceiptTransfer.Services;

namespace Wms.WebApi.Controllers.SubcontractingReceiptTransfer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class SrtFunctionController : ControllerBase
{
    private readonly ISrtFunctionService _service;

    public SrtFunctionController(ISrtFunctionService service)
    {
        _service = service;
    }

    [HttpGet("headers/{customerCode}")]
    public async Task<ActionResult<ApiResponse<List<SrtOpenOrderHeaderDto>>>> GetSrtOpenOrderHeader(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetSrtOpenOrderHeaderAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/{siparisNoCsv}")]
    public async Task<ActionResult<ApiResponse<List<SrtOpenOrderLineDto>>>> GetSrtOpenOrderLine(string siparisNoCsv, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetSrtOpenOrderLineAsync(siparisNoCsv, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
