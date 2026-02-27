using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlatformPageGroupController : ControllerBase
    {
        private readonly IPlatformPageGroupService _platformPageGroupService;

        public PlatformPageGroupController(IPlatformPageGroupService platformPageGroupService)
        {
            _platformPageGroupService = platformPageGroupService;
        }

        /// <summary>
        /// Tüm PlatformPageGroup kayıtlarını getirir
        /// </summary>
        /// <returns>PlatformPageGroup listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformPageGroupDto>>> GetAll()
        {
            var result = await _platformPageGroupService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre PlatformPageGroup kaydını getirir
        /// </summary>
        /// <param name="id">PlatformPageGroup ID</param>
        /// <returns>PlatformPageGroup detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformPageGroupDto>> GetById(long id)
        {
            var result = await _platformPageGroupService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni PlatformPageGroup kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak PlatformPageGroup bilgileri</param>
        /// <returns>Oluşturulan PlatformPageGroup</returns>
        [HttpPost]
        public async Task<ActionResult<PlatformPageGroupDto>> Create([FromBody] CreatePlatformPageGroupDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _platformPageGroupService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// PlatformPageGroup kaydını günceller
        /// </summary>
        /// <param name="id">PlatformPageGroup ID</param>
        /// <param name="updateDto">Güncellenecek veriler</param>
        /// <returns>Güncellenmiş PlatformPageGroup</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PlatformPageGroupDto>> Update(long id, [FromBody] UpdatePlatformPageGroupDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _platformPageGroupService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// PlatformPageGroup kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek PlatformPageGroup ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _platformPageGroupService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// GroupCode'a göre PlatformPageGroup kayıtlarını getirir
        /// </summary>
        /// <param name="groupCode">Group Code</param>
        /// <returns>PlatformPageGroup listesi</returns>
        [HttpGet("by-group-code/{groupCode}")]
        public async Task<ActionResult<IEnumerable<PlatformPageGroupDto>>> GetByGroupCode(string groupCode)
        {
            var result = await _platformPageGroupService.GetByGroupCodeAsync(groupCode);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// MenuHeaderId'ye göre PlatformPageGroup kayıtlarını getirir
        /// </summary>
        /// <param name="menuHeaderId">Menu Header ID</param>
        /// <returns>PlatformPageGroup listesi</returns>
        [HttpGet("by-menu-header/{menuHeaderId}")]
        public async Task<ActionResult<IEnumerable<PlatformPageGroupDto>>> GetByMenuHeaderId(int menuHeaderId)
        {
            var result = await _platformPageGroupService.GetByMenuHeaderIdAsync(menuHeaderId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// MenuLineId'ye göre PlatformPageGroup kayıtlarını getirir
        /// </summary>
        /// <param name="menuLineId">Menu Line ID</param>
        /// <returns>PlatformPageGroup listesi</returns>
        [HttpGet("by-menu-line/{menuLineId}")]
        public async Task<ActionResult<IEnumerable<PlatformPageGroupDto>>> GetByMenuLineId(int menuLineId)
        {
            var result = await _platformPageGroupService.GetByMenuLineIdAsync(menuLineId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GroupCode'a göre gruplandırılmış sayfa gruplarını getirir (her GroupCode için ilk kaydı)
        /// </summary>
        /// <returns>GroupCode'a göre gruplandırılmış PlatformPageGroup listesi</returns>
        [HttpGet("grouped-by-group-code")]
        public async Task<ActionResult<IEnumerable<PlatformPageGroupDto>>> GetPageGroupsGroupByGroupCode()
        {
            var result = await _platformPageGroupService.GetPageGroupsGroupByGroupCodeAsync();
            return StatusCode(result.StatusCode, result);
        }
    }
}