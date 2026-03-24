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
    public class GrLineController : ControllerBase
    {
        private readonly IGrLineService _grLineService;
        private readonly ILocalizationService _localizationService;

        public GrLineController(IGrLineService grLineService, ILocalizationService localizationService)
        {
            _grLineService = grLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm GR Line kayıtlarını getirir
        /// </summary>
        /// <returns>GR Line listesi</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grLineService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre GR Line kaydını getirir
        /// </summary>
        /// <param name="id">GR Line ID</param>
        /// <returns>GR Line detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrLineDto>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grLineService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Header ID'ye göre GR Line kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">GR Header ID</param>
        /// <returns>GR Line listesi</returns>
        [HttpPost("by-header/{headerId}/paged")]
        public async Task<IActionResult> GetByHeaderId(long headerId, [FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grLineService.GetByHeaderIdAsync(headerId, cancellationToken);
            var pagedResult = result.ToPagedResponse(request);
            return StatusCode(pagedResult.StatusCode, pagedResult);
        }

        /// <summary>
        /// Yeni GR Line kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak GR Line bilgileri</param>
        /// <returns>Oluşturulan GR Line</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrLineDto>>> Create([FromBody] CreateGrLineDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grLineService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut GR Line kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek GR Line ID</param>
        /// <param name="updateDto">Güncellenecek GR Line bilgileri</param>
        /// <returns>Güncellenmiş GR Line</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrLineDto>>> Update(long id, [FromBody] UpdateGrLineDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ApiResponse<GrLineDto>.ErrorResult(_localizationService.GetLocalizedString("InvalidModelState"), ModelState?.ToString() ?? string.Empty, 400));
            }

            var result = await _grLineService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GR Line kaydını soft delete yapar
        /// </summary>
        /// <param name="id">Silinecek GR Line ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grLineService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grLineService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
