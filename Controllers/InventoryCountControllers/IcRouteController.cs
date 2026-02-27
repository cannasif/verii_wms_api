using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IcRouteController : ControllerBase
    {
        private readonly IIcRouteService _service;
        private readonly ILocalizationService _localizationService;

        public IcRouteController(IIcRouteService service, ILocalizationService localizationService)
        {
            _service = service;
            _localizationService = localizationService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<IcRouteDto>>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<IcRouteDto>>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("importline/{importLineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<IcRouteDto>>>> GetByImportLineId(long importLineId)
        {
            var result = await _service.GetByImportLineIdAsync(importLineId);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<IcRouteDto>>> Create([FromBody] CreateIcRouteDto createDto)
        {
            var result = await _service.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<IcRouteDto>>> Update(long id, [FromBody] UpdateIcRouteDto updateDto)
        {
            var result = await _service.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(long id)
        {
            var result = await _service.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}