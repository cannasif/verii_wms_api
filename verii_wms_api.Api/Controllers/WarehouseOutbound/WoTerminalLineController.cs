using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WoTerminalLineController : ControllerBase
    {
        private readonly IWoTerminalLineService _service;
        private readonly ILocalizationService _localizationService;

        public WoTerminalLineController(IWoTerminalLineService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<IActionResult> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(long userId, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetByUserIdAsync(userId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWoTerminalLineDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateWoTerminalLineDto updateDto, CancellationToken cancellationToken = default)
        {
            var result = await _service.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _service.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
