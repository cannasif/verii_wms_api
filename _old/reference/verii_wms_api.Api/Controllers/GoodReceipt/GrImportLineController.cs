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
    public class GrImportLineController : ControllerBase
    {
        private readonly IGrImportLineService _grImportLineService;
        private readonly ILocalizationService _localizationService;

        public GrImportLineController(IGrImportLineService grImportLineService, ILocalizationService localizationService)
        {
            _grImportLineService = grImportLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm GrImportLine kayıtlarını getirir
        /// </summary>
        /// <returns>GrImportLine listesi</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre GrImportLine kaydını getirir
        /// </summary>
        /// <param name="id">GrImportLine ID</param>
        /// <returns>GrImportLine detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Header ID'ye göre GrImportLine kayıtlarını getirir
        /// </summary>
        /// <param name="headerId">Header ID</param>
        /// <returns>GrImportLine listesi</returns>
        [HttpGet("by-header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-header-with-routes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>>> GetWithRoutesByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetWithRoutesByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("goodReceiptOrderCollectedBarcodes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineWithRoutesDto>>>> GoodReceiptOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Line ID'ye göre GrImportLine kayıtlarını getirir
        /// </summary>
        /// <param name="lineId">Line ID</param>
        /// <returns>GrImportLine listesi</returns>
        [HttpGet("by-line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<GrImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetByLineIdAsync(lineId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }



        /// <summary>
        /// Yeni GrImportLine kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak GrImportLine bilgileri</param>
        /// <returns>Oluşturulan GrImportLine</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<GrImportLineDto>>> Create([FromBody] CreateGrImportLineDto createDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _grImportLineService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut GrImportLine kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek GrImportLine ID</param>
        /// <param name="updateDto">Güncellenecek GrImportLine bilgileri</param>
        /// <returns>Güncellenmiş GrImportLine</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GrImportLineDto>>> Update(long id, [FromBody] UpdateGrImportLineDto updateDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _grImportLineService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GrImportLine kaydını soft delete yapar
        /// </summary>
        /// <param name="id">Silinecek GrImportLine ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<GrImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _grImportLineService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

