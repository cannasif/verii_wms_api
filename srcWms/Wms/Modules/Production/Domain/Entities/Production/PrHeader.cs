using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Production;

public sealed class PrHeader : BaseHeaderEntity
{
    public string? CustomerCode { get; set; }
    public long? CustomerId { get; set; }
    public string? StockCode { get; set; }
    public long? StockId { get; set; }
    public string? YapKod { get; set; }
    public long? YapKodId { get; set; }
    public decimal? Quantity { get; set; }
    public string? SourceWarehouse { get; set; }
    public long? SourceWarehouseId { get; set; }
    public string? TargetWarehouse { get; set; }
    public long? TargetWarehouseId { get; set; }

    public string PlanType { get; set; } = "Production";
    public string ExecutionMode { get; set; } = "Serial";
    public string Status { get; set; } = "Draft";
    public int Priority { get; set; }
    public long? ProjectId { get; set; }
    public string? MainStockCode { get; set; }
    public long? MainStockId { get; set; }
    public string? MainYapKod { get; set; }
    public long? MainYapKodId { get; set; }
    public decimal? PlannedQuantity { get; set; }
    public decimal? CompletedQuantity { get; set; }
    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public long? ReleasedBy { get; set; }

    public ICollection<PrLine> Lines { get; set; } = new List<PrLine>();
    public ICollection<PrImportLine> ImportLines { get; set; } = new List<PrImportLine>();
    public ICollection<PrHeaderAssignment> Assignments { get; set; } = new List<PrHeaderAssignment>();
    public ICollection<PrOrder> Orders { get; set; } = new List<PrOrder>();
}
