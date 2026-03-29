using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;
using WMS_WEBAPI.Services;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserAuthorityController : ControllerBase
    {
        private readonly IUserAuthorityService _userAuthorityService;
        private readonly ILocalizationService _localizationService;

        public UserAuthorityController(IUserAuthorityService userAuthorityService, ILocalizationService localizationService)
        {
            _userAuthorityService = userAuthorityService;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userAuthorityService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<UserAuthorityDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _userAuthorityService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApiResponse<UserAuthorityDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _userAuthorityService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserAuthorityDto>>> Create([FromBody] CreateUserAuthorityDto createDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _userAuthorityService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ApiResponse<UserAuthorityDto>>> Update(long id, [FromBody] UpdateUserAuthorityDto updateDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _userAuthorityService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _userAuthorityService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        
    }
}
