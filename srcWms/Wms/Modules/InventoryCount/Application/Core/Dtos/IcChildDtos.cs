using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.InventoryCount.Dtos;

public sealed class IcImportLineDto : BaseImportLineEntityDto
{
    public long HeaderId { get; set; }
}

public sealed class IcImportLineWithRoutesDto
{
    public IcImportLineDto ImportLine { get; set; } = null!;
    public List<IcRouteDto> Routes { get; set; } = new();
}

public sealed class CreateIcImportLineDto : BaseImportLineCreateDto
{
    [Required] public long HeaderId { get; set; }
}

public sealed class UpdateIcImportLineDto : BaseImportLineUpdateDto
{
    public long? HeaderId { get; set; }
}

public sealed class IcRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string YapKod { get; set; } = string.Empty;
    public int Priority { get; set; }
}

public sealed class CreateIcRouteDto : BaseRouteCreateDto
{
    [Required] public long ImportLineId { get; set; }
    [StringLength(30)] public string YapKod { get; set; } = string.Empty;
    public int Priority { get; set; }
    [StringLength(100)] public string? Description { get; set; }
    public decimal OldQuantity { get; set; }
    public decimal NewQuantity { get; set; }
    [StringLength(50)] public string? Barcode { get; set; }
    public int? OldWarehouse { get; set; }
    public int? NewWarehouse { get; set; }
}

public sealed class UpdateIcRouteDto
{
    public long? ImportLineId { get; set; }
    [StringLength(30)] public string? YapKod { get; set; }
    public int? Priority { get; set; }
    [StringLength(100)] public string? Description { get; set; }
    public decimal? OldQuantity { get; set; }
    public decimal? NewQuantity { get; set; }
    [StringLength(50)] public string? Barcode { get; set; }
    public int? OldWarehouse { get; set; }
    public int? NewWarehouse { get; set; }
}

public sealed class IcTerminalLineDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public long TerminalUserId { get; set; }
}

public sealed class CreateIcTerminalLineDto : BaseTerminalLineCreateDto
{
    [Required] public long HeaderId { get; set; }
}

public sealed class UpdateIcTerminalLineDto
{
    public long? HeaderId { get; set; }
    public long? TerminalUserId { get; set; }
}
