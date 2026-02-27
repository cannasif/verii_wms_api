using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WtFunctionController : ControllerBase
    {
        private readonly IWtFunctionService _wtFunctionService;

        public WtFunctionController(IWtFunctionService wtFunctionService)
        {
            _wtFunctionService = wtFunctionService;
        }

        [HttpGet("headers/{customerCode}")]
        public async Task<ActionResult<ApiResponse<List<TransferOpenOrderHeaderDto>>>> GetTransferOpenOrderHeader(string customerCode)
        {
            var result = await _wtFunctionService.GetTransferOpenOrderHeaderAsync(customerCode);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("lines/{siparisNoCsv}")]
        public async Task<ActionResult<ApiResponse<List<TransferOpenOrderLineDto>>>> GetTransferOpenOrderLine(string siparisNoCsv)
        {
            var result = await _wtFunctionService.GetTransferOpenOrderLineAsync(siparisNoCsv);
            return StatusCode(result.StatusCode, result);
        }
    }
}