using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Interfaces;

namespace WMS_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlatformUserGroupMatchController : ControllerBase
    {
        private readonly IPlatformUserGroupMatchService _platformUserGroupMatchService;

        public PlatformUserGroupMatchController(IPlatformUserGroupMatchService platformUserGroupMatchService)
        {
            _platformUserGroupMatchService = platformUserGroupMatchService;
        }

        /// <summary>
        /// Tüm PlatformUserGroupMatch kayıtlarını getirir
        /// </summary>
        /// <returns>PlatformUserGroupMatch listesi</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformUserGroupMatchDto>>> GetAll()
        {
            var result = await _platformUserGroupMatchService.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// ID'ye göre PlatformUserGroupMatch kaydını getirir
        /// </summary>
        /// <param name="id">PlatformUserGroupMatch ID</param>
        /// <returns>PlatformUserGroupMatch detayı</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlatformUserGroupMatchDto>> GetById(long id)
        {
            var result = await _platformUserGroupMatchService.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Yeni PlatformUserGroupMatch kaydı oluşturur
        /// </summary>
        /// <param name="createDto">Oluşturulacak PlatformUserGroupMatch bilgileri</param>
        /// <returns>Oluşturulan PlatformUserGroupMatch</returns>
        [HttpPost]
        public async Task<ActionResult<PlatformUserGroupMatchDto>> Create([FromBody] CreatePlatformUserGroupMatchDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _platformUserGroupMatchService.CreateAsync(createDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Mevcut PlatformUserGroupMatch kaydını günceller
        /// </summary>
        /// <param name="id">Güncellenecek PlatformUserGroupMatch ID</param>
        /// <param name="updateDto">Güncellenecek PlatformUserGroupMatch bilgileri</param>
        /// <returns>Güncellenmiş PlatformUserGroupMatch</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<PlatformUserGroupMatchDto>> Update(long id, [FromBody] UpdatePlatformUserGroupMatchDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var result = await _platformUserGroupMatchService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// PlatformUserGroupMatch kaydını siler (soft delete)
        /// </summary>
        /// <param name="id">Silinecek PlatformUserGroupMatch ID</param>
        /// <returns>Silme işlemi sonucu</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _platformUserGroupMatchService.SoftDeleteAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        

        /// <summary>
        /// UserId'ye göre PlatformUserGroupMatch kayıtlarını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>PlatformUserGroupMatch listesi</returns>
        [HttpGet("by-user/{userId}")]
        public async Task<ActionResult<IEnumerable<PlatformUserGroupMatchDto>>> GetByUserId(int userId)
        {
            var result = await _platformUserGroupMatchService.GetByUserIdAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// GroupCode'a göre PlatformUserGroupMatch kayıtlarını getirir
        /// </summary>
        /// <param name="groupCode">Grup kodu</param>
        /// <returns>PlatformUserGroupMatch listesi</returns>
        [HttpGet("by-group-code/{groupCode}")]
        public async Task<ActionResult<IEnumerable<PlatformUserGroupMatchDto>>> GetByGroupCode(string groupCode)
        {
            var result = await _platformUserGroupMatchService.GetByGroupCodeAsync(groupCode);
            return StatusCode(result.StatusCode, result);
        }
    }
}