using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcAdjustment : BaseEntity
{
    public long HeaderId { get; set; }
    public IcHeader Header { get; set; } = null!;
    public long LineId { get; set; }
    public IcLine Line { get; set; } = null!;
    public decimal ExpectedQuantity { get; set; }
    public decimal ApprovedCountedQuantity { get; set; }
    public decimal DifferenceQuantity { get; set; }
    public string AdjustmentType { get; set; } = "None";
    public string Status { get; set; } = "Pending";
    public long? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ErpReferenceNo { get; set; }
    public DateTime? PostingDate { get; set; }
    public string? Note { get; set; }
}
