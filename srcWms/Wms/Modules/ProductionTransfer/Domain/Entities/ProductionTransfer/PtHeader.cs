using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? TargetWarehouse { get; set; }
    public ICollection<PtLine> Lines { get; set; } = new List<PtLine>();
    public ICollection<PtImportLine> ImportLines { get; set; } = new List<PtImportLine>();
}
