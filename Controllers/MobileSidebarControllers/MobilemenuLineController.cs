using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobilemenuLineController : ControllerBase
    {
        private readonly IMobilemenuLineService _mobilemenuLineService;
        private readonly ILocalizationService _localizationService;

        public MobilemenuLineController(IMobilemenuLineService mobilemenuLineService, ILocalizationService localizationService)
        {
            _mobilemenuLineService = mobilemenuLineService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilemenuLineDto>>>> GetAll()
        {
            var result = await _mobilemenuLineService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MobilemenuLineDto>>> GetById(long id)
        {
            var result = await _mobilemenuLineService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-item-id/{itemId}")]
        public async Task<ActionResult<ApiResponse<MobilemenuLineDto>>> GetByItemId(string itemId)
        {
            var result = await _mobilemenuLineService.GetByItemIdAsync(itemId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilemenuLineDto>>>> GetByHeaderId(int headerId)
        {
            var result = await _mobilemenuLineService.GetByHeaderIdAsync(headerId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-title/{title}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilemenuLineDto>>>> GetByTitle(string title)
        {
            var result = await _mobilemenuLineService.GetByTitleAsync(title);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<MobilemenuLineDto>>> Create([FromBody] CreateMobilemenuLineDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var message = _localizationService.GetLocalizedString("InvalidModelState");
                var errorResult = ApiResponse<MobilemenuLineDto>.ErrorResult(message, "Model validation failed", 400);
                return StatusCode(errorResult.StatusCode, errorResult);
            }

            var result = await _mobilemenuLineService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MobilemenuLineDto>>> Update(long id, [FromBody] UpdateMobilemenuLineDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var message = _localizationService.GetLocalizedString("InvalidModelState");
                var errorResult = ApiResponse<MobilemenuLineDto>.ErrorResult(message, "Model validation failed", 400);
                return StatusCode(errorResult.StatusCode, errorResult);
            }

            var result = await _mobilemenuLineService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _mobilemenuLineService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}