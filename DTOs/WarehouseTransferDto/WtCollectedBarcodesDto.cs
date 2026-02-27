using System.Collections.Generic;

namespace WMS_WEBAPI.DTOs
{
    public class WtImportLineWithRoutesDto
    {
        public WtImportLineDto ImportLine { get; set; } = null!;
        public List<WtRouteDto> Routes { get; set; } = new();
    }
}