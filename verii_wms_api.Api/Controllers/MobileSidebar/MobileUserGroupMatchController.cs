using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobileUserGroupMatchController : ControllerBase
    {
        private readonly IMobileUserGroupMatchService _mobileUserGroupMatchService;
        private readonly ILocalizationService _localizationService;

        public MobileUserGroupMatchController(IMobileUserGroupMatchService mobileUserGroupMatchService, ILocalizationService localizationService)
        {
            _mobileUserGroupMatchService = mobileUserGroupMatchService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _mobileUserGroupMatchService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<MobileUserGroupMatchDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _mobileUserGroupMatchService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MobileUserGroupMatchDto>>> GetById(long id)
        {
            var result = await _mobileUserGroupMatchService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>>> GetByUserId(int userId)
        {
            var result = await _mobileUserGroupMatchService.GetByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-group-code/{groupCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobileUserGroupMatchDto>>>> GetByGroupCode(string groupCode)
        {
            var result = await _mobileUserGroupMatchService.GetByGroupCodeAsync(groupCode);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<MobileUserGroupMatchDto>>> Create([FromBody] CreateMobileUserGroupMatchDto createDto)
        {
            

            var result = await _mobileUserGroupMatchService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MobileUserGroupMatchDto>>> Update(long id, [FromBody] UpdateMobileUserGroupMatchDto updateDto)
        {
            

            var result = await _mobileUserGroupMatchService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _mobileUserGroupMatchService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}