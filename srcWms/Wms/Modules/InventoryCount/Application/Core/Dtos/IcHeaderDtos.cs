using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.InventoryCount.Dtos;

public sealed class IcHeaderDto : BaseHeaderEntityDto
{
    public string? CellCode { get; set; }
    public string? WarehouseCode { get; set; }
    public string? ProductCode { get; set; }
    public byte Type { get; set; }
}

public sealed class CreateIcHeaderDto : BaseHeaderCreateDto
{
    [StringLength(35)] public string? CellCode { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    [StringLength(50)] public string? ProductCode { get; set; }
    [Required] public byte Type { get; set; }
}

public sealed class UpdateIcHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(35)] public string? CellCode { get; set; }
    [StringLength(20)] public string? WarehouseCode { get; set; }
    [StringLength(50)] public string? ProductCode { get; set; }
    public byte? Type { get; set; }
}
