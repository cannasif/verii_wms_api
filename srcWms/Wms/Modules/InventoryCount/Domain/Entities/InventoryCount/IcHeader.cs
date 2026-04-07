using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.InventoryCount;

public sealed class IcHeader : BaseHeaderEntity
{
    public string CountType { get; set; } = "General";
    public string ScopeMode { get; set; } = "All";
    public string CountMode { get; set; } = "Blind";
    public string Status { get; set; } = "Draft";
    public string FreezeMode { get; set; } = "None";

    public string? WarehouseCode { get; set; }
    public long? WarehouseId { get; set; }
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public string? RackCode { get; set; }
    public string? CellCode { get; set; }

    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? StartedDate { get; set; }
    public int? SequenceNo { get; set; }

    public bool IsFirstCount { get; set; } = true;
    public bool RequiresRecount { get; set; }

    public long? AssignedUserId { get; set; }
    public long? AssignedRoleId { get; set; }
    public long? AssignedTeamId { get; set; }

    public int? LineCount { get; set; }
    public int? CountedLineCount { get; set; }
    public int? DifferenceLineCount { get; set; }
    public int? RecountRequiredLineCount { get; set; }

    public ICollection<IcTerminalLine> TerminalLines { get; set; } = new List<IcTerminalLine>();
    public ICollection<IcImportLine> ImportLines { get; set; } = new List<IcImportLine>();
    public ICollection<IcScope> Scopes { get; set; } = new List<IcScope>();
    public ICollection<IcLine> Lines { get; set; } = new List<IcLine>();
    public ICollection<IcCountEntry> CountEntries { get; set; } = new List<IcCountEntry>();
    public ICollection<IcAdjustment> Adjustments { get; set; } = new List<IcAdjustment>();
}
