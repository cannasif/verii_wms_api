using Wms.Domain.Entities.Common;
using Wms.Domain.Entities.Production;

namespace Wms.Domain.Entities.ProductionTransfer;

public sealed class PtLine : BaseLineEntity
{
    public long HeaderId { get; set; }
    public PtHeader Header { get; set; } = null!;
    public long? ProductionOrderId { get; set; }
    public PrOrder? ProductionOrder { get; set; }
    public long? ProductionOrderOutputId { get; set; }
    public PrOrderOutput? ProductionOrderOutput { get; set; }
    public long? ProductionOrderConsumptionId { get; set; }
    public PrOrderConsumption? ProductionOrderConsumption { get; set; }
    public string? LineRole { get; set; }
    public decimal? RequiredQuantityFromProduction { get; set; }
    public ICollection<PtImportLine> ImportLines { get; set; } = new List<PtImportLine>();
    public ICollection<PtLineSerial> SerialLines { get; set; } = new List<PtLineSerial>();
}
