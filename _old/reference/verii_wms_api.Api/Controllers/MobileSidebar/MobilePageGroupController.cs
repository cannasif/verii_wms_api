using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobilePageGroupController : ControllerBase
    {
        private readonly IMobilePageGroupService _mobilePageGroupService;
        private readonly ILocalizationService _localizationService;

        public MobilePageGroupController(IMobilePageGroupService mobilePageGroupService, ILocalizationService localizationService)
        {
            _mobilePageGroupService = mobilePageGroupService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request)
        {
            var result = await _mobilePageGroupService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<MobilePageGroupDto>>>> GetPaged([FromBody] PagedRequest request)
        {
            var result = await _mobilePageGroupService.GetPagedAsync(request);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MobilePageGroupDto>>> GetById(long id)
        {
            var result = await _mobilePageGroupService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-group-code/{groupCode}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilePageGroupDto>>>> GetByGroupCode(string groupCode)
        {
            var result = await _mobilePageGroupService.GetByGroupCodeAsync(groupCode);
            return StatusCode(result.StatusCode, result);
        }

        // Yeni: GroupCode'a göre gruplandırılmış sayfa grupları (her GroupCode için ilk kayıt)
        [HttpGet("grouped-by-group-code")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MobilePageGroupDto>>>> GetPageGroupsGroupedByGroupCode()
        {
            var result = await _mobilePageGroupService.GetMobilPageGroupsByGroupCodeAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MobilePageGroupDto>>> Create([FromBody] CreateMobilePageGroupDto createDto)
        {
            

            var result = await _mobilePageGroupService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MobilePageGroupDto>>> Update(long id, [FromBody] UpdateMobilePageGroupDto updateDto)
        {
            

            var result = await _mobilePageGroupService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id)
        {
            var result = await _mobilePageGroupService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
