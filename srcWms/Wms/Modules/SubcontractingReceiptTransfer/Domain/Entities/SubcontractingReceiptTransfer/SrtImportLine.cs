using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.SubcontractingReceiptTransfer;

public sealed class SrtImportLine : BaseImportLineEntity
{
    public long HeaderId { get; set; }
    public SrtHeader Header { get; set; } = null!;
    public long? LineId { get; set; }
    public SrtLine? Line { get; set; }
    public ICollection<SrtRoute> Routes { get; set; } = new List<SrtRoute>();
}
