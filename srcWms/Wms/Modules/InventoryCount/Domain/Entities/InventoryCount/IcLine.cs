using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcLine : BaseEntity
{
    public long HeaderId { get; set; }
    public IcHeader Header { get; set; } = null!;
    public long? ScopeId { get; set; }
    public IcScope? Scope { get; set; }
    public int SequenceNo { get; set; }

    public long? WarehouseId { get; set; }
    public string? WarehouseCode { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }

    public long StockId { get; set; }
    public string StockCode { get; set; } = null!;
    public long? YapKodId { get; set; }
    public string? YapKod { get; set; }

    public string? LotNo { get; set; }
    public string? SerialNo1 { get; set; }
    public string? SerialNo2 { get; set; }
    public string? BatchNo { get; set; }
    public string? Unit { get; set; }

    public decimal ExpectedQuantity { get; set; }
    public decimal? CountedQuantity { get; set; }
    public decimal? DifferenceQuantity { get; set; }

    public string CountStatus { get; set; } = "Pending";
    public int EntryCount { get; set; }
    public bool IsMatched { get; set; }
    public bool IsDifference { get; set; }
    public bool IsExtraStock { get; set; }
    public bool IsMissingStock { get; set; }
    public bool IsRecountRequired { get; set; }
    public DateTime? FirstCountedAt { get; set; }
    public DateTime? LastCountedAt { get; set; }
    public long? CountedByUserId { get; set; }
    public string? ApprovalNote { get; set; }
    public string? DifferenceReason { get; set; }

    public ICollection<IcCountEntry> CountEntries { get; set; } = new List<IcCountEntry>();
    public ICollection<IcAdjustment> Adjustments { get; set; } = new List<IcAdjustment>();
}
