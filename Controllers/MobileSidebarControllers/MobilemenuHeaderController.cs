using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobilemenuHeaderController : ControllerBase
    {
        private readonly IMobilemenuHeaderService _mobilemenuHeaderService;
        private readonly ILocalizationService _localizationService;

        public MobilemenuHeaderController(IMobilemenuHeaderService mobilemenuHeaderService, ILocalizationService localizationService)
        {
            _mobilemenuHeaderService = mobilemenuHeaderService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilemenuHeaderDto>>>> GetAll()
        {
            var result = await _mobilemenuHeaderService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MobilemenuHeaderDto>>> GetById(long id)
        {
            var result = await _mobilemenuHeaderService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-menu-id/{menuId}")]
        public async Task<ActionResult<ApiResponse<MobilemenuHeaderDto>>> GetByMenuId(string menuId)
        {
            var result = await _mobilemenuHeaderService.GetByMenuIdAsync(menuId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-title/{title}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilemenuHeaderDto>>>> GetByTitle(string title)
        {
            var result = await _mobilemenuHeaderService.GetByTitleAsync(title);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("open-menus")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilemenuHeaderDto>>>> GetOpenMenus()
        {
            var result = await _mobilemenuHeaderService.GetOpenMenusAsync();
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<MobilemenuHeaderDto>>> Create([FromBody] CreateMobilemenuHeaderDto createDto)
        {
            if (!ModelState.IsValid)
            {
                var message = _localizationService.GetLocalizedString("InvalidModelState");
                var errorResult = ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, "Model validation failed", 400);
                return StatusCode(errorResult.StatusCode, errorResult);
            }

            var result = await _mobilemenuHeaderService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MobilemenuHeaderDto>>> Update(long id, [FromBody] UpdateMobilemenuHeaderDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                var message = _localizationService.GetLocalizedString("InvalidModelState");
                var errorResult = ApiResponse<MobilemenuHeaderDto>.ErrorResult(message, "Model validation failed", 400);
                return StatusCode(errorResult.StatusCode, errorResult);
            }

            var result = await _mobilemenuHeaderService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _mobilemenuHeaderService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}