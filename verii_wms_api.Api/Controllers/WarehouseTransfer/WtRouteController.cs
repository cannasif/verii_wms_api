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
    public class WtRouteController : ControllerBase
    {
        private readonly IWtRouteService _wtRouteService;
        private readonly ILocalizationService _localizationService;

        public WtRouteController(IWtRouteService wtRouteService, ILocalizationService localizationService)
        {
            _wtRouteService = wtRouteService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Tüm WtRoute kayıtlarını getirir
        /// </summary>
        /// <returns>WtRoute listesi</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtRouteService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("paged")]
        public async Task<ActionResult<ApiResponse<PagedResponse<WtRouteDto>>>> GetPaged([FromBody] PagedRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _wtRouteService.GetPagedAsync(request, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// ID'ye göre WtRoute kaydını getirir
        /// </summary>
        /// <param name="id">WtRoute ID</param>
        /// <returns>WtRoute detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WtRouteDto?>>> GetById(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtRouteService.GetByIdAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Line ID'ye göre WtRoute kayıtlarını getirir
        /// </summary>
        /// <param name="lineId">Line ID</param>
        /// <returns>WtRoute listesi</returns>
        


        /// <summary>
        /// Seri numarasına göre WtRoute kayıtlarını getirir
        /// </summary>
        /// <param name="serialNo">Seri numarası</param>
        /// <returns>WtRoute listesi</returns>
        [HttpGet("serial/{serialNo}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<WtRouteDto>>>> GetBySerialNo(string serialNo, CancellationToken cancellationToken = default)
        {
            var result = await _wtRouteService.GetBySerialNoAsync(serialNo, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Kaynak depo koduna göre WtRoute kayıtlarını getirir
        /// </summary>
        /// <param name="sourceWarehouse">Kaynak depo kodu</param>
        /// <returns>WtRoute listesi</returns>

        /// <summary>
        /// Hedef depo koduna göre WtRoute kayıtlarını getirir
        /// </summary>
        /// <param name="targetWarehouse">Hedef depo kodu</param>
        /// <returns>WtRoute listesi</returns>

        /// <summary>
        /// Yeni WtRoute kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak WtRoute bilgileri</param>
        /// <returns>Oluşturulan WtRoute</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<WtRouteDto>>> Create([FromBody] CreateWtRouteDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtRouteService.CreateAsync(createDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtRoute kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek WtRoute ID</param>
        /// <param name="updateDto">Güncellenecek WtRoute bilgileri</param>
        /// <returns>Güncellenmiş WtRoute</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WtRouteDto>>> Update(long id, [FromBody] UpdateWtRouteDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _wtRouteService.UpdateAsync(id, updateDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// WtRoute kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek WtRoute ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(long id, CancellationToken cancellationToken = default)
        {
            var result = await _wtRouteService.SoftDeleteAsync(id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        /// <summary>
        /// Miktar aralığına göre WtRoute kayıtlarını getirir
        /// </summary>
        /// <param name="minQuantity">Minimum miktar</param>
        /// <param name="maxQuantity">Maksimum miktar</param>
        /// <returns>WtRoute listesi</returns>
    }
}
