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
    public class WtImportLineController : ControllerBase
    {
        private readonly IWtImportLineService _wtImportLineService;
        private readonly ILocalizationService _localizationService;

        public WtImportLineController(IWtImportLineService wtImportLineService, ILocalizationService localizationService)
        {
            _wtImportLineService = wtImportLineService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm WtImportLine kayıtlarını getirir
        /// </summary>
        /// <returns>WtImportLine listesi</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtImportLineDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// ID'ye göre TrImportLine kaydını getirir
        /// </summary>
        /// <param name="id">WtImportLine ID</param>
        /// <returns>WtImportLine detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtImportLineDto?>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Line ID'ye göre WtImportLine kayıtlarını getirir
        /// </summary>
        /// <param name="lineId">Line ID</param>
        /// <returns>WtImportLine listesi</returns>
        [HttpGet("line/{lineId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineDto>>>> GetByLineId(long lineId, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetByLineIdAsync(lineId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("header/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineDto>>>> GetByHeaderId(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Route ID'ye göre WtImportLine kayıtlarını getirir
        /// </summary>
        /// <param name="routeId">Route ID</param>
        /// <returns>WtImportLine listesi</returns>
        [HttpGet("route/{routeId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineDto>>>> GetByRouteId(long routeId, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetByRouteIdAsync(routeId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }




        /// <summary>
        /// Yeni WtImportLine kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak WtImportLine bilgileri</param>
        /// <returns>Oluşturulan WtImportLine</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<WtImportLineDto>>> Create([FromBody] CreateWtImportLineDto createDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _wtImportLineService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtImportLine kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek WtImportLine ID</param>
        /// <param name="updateDto">Güncellenecek WtImportLine bilgileri</param>
        /// <returns>Güncellenmiş WtImportLine</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WtImportLineDto>>> Update(long id, [FromBody] UpdateWtImportLineDto updateDto, CancellationToken cancellationToken = default)
        {
            

            var result = await _wtImportLineService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtImportLine kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek WtImportLine ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("addBarcodeBasedonAssignedOrder")]
        public async Task<ActionResult<ApiResponse<WtImportLineDto>>> AddBarcodeBasedonAssignedOrder([FromBody] AddWtImportBarcodeRequestDto request, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.AddBarcodeBasedonAssignedOrderAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("warehouseTransferOrderCollectedBarcodes/{headerId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtImportLineWithRoutesDto>>>> WarehouseTransferOrderCollectedBarcodes(long headerId, CancellationToken cancellationToken = default)
        {
            var result = await _wtImportLineService.GetCollectedBarcodesByHeaderIdAsync(headerId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

    }
}
