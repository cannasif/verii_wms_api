using WMS_WEBAPI.Models;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Interfaces
{
    public interface IJwtService
    {
        ApiResponse<string> GenerateToken(User user);
    }
}