using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Application.GoodsReceipt.Services;

namespace Wms.Modules.GoodsReceipt.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class GoodReciptFunctionsController : ControllerBase
{
    private readonly IGoodReciptFunctionsService _service;

    public GoodReciptFunctionsController(IGoodReciptFunctionsService service)
    {
        _service = service;
    }

    [HttpGet("headers/customer/{customerCode}")]
    public async Task<ActionResult<ApiResponse<List<GoodsOpenOrdersHeaderDto>>>> GetGoodsReceiptHeadersByCustomer(string customerCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetGoodsReceiptHeaderAsync(customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/customer/{customerCode}/orders/{siparisNoCsv?}")]
    public async Task<ActionResult<ApiResponse<List<GoodsOpenOrdersLineDto>>>> GetGoodsReceiptLines(string customerCode, string? siparisNoCsv = null, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetGoodsReceiptLineAsync(siparisNoCsv ?? string.Empty, customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("lines/customer/{customerCode}/branch/{branchCode}")]
    public async Task<ActionResult<ApiResponse<List<GoodsOpenOrdersLineDto>>>> GetGoodsReceiptLinesByCustomerAndBranch(string customerCode, string branchCode, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetGoodsReceiptLineByCustomerCodeAndBranchCodeAsync(branchCode, customerCode, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
