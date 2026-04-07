using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.ProductionTransfer.Dtos;

public sealed class PtHeaderDto : BaseHeaderEntityDto
{
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? SourceWarehouse { get; set; }
    public string? SourceWarehouseName { get; set; }
    public string? TargetWarehouse { get; set; }
    public string? TargetWarehouseName { get; set; }
    public string? TransferPurpose { get; set; }
    public long? ProductionHeaderId { get; set; }
    public long? ProductionOrderId { get; set; }
    public string? DocumentNo { get; set; }
    public DateTime? DocumentDate { get; set; }
    public bool CanDelete { get; set; }
    public string? DeleteBlockedReason { get; set; }
}

public sealed class CreateProductionTransferRequestDto
{
    [Required, StringLength(30)]
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    [Required, StringLength(30)]
    public string TransferPurpose { get; set; } = "MaterialSupply";
    [StringLength(30)]
    public string? ProductionDocumentNo { get; set; }
    [StringLength(30)]
    public string? ProductionOrderNo { get; set; }
    [StringLength(20)]
    public string SourceWarehouseCode { get; set; } = string.Empty;
    [StringLength(20)]
    public string TargetWarehouseCode { get; set; } = string.Empty;
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    public List<CreateProductionTransferLineDto> Lines { get; set; } = new();
}

public sealed class CreateProductionTransferLineDto
{
    [Required]
    public string LocalId { get; set; } = string.Empty;
    [Required, StringLength(50)]
    public string StockCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string YapKod { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    [Required, StringLength(20)]
    public string LineRole { get; set; } = "ConsumptionSupply";
    [StringLength(50)]
    public string SourceCellCode { get; set; } = string.Empty;
    [StringLength(50)]
    public string TargetCellCode { get; set; } = string.Empty;
    [StringLength(30)]
    public string ProductionOrderNo { get; set; } = string.Empty;
}

public sealed class ProductionTransferSuggestionRequestDto
{
    [StringLength(30)]
    public string? ProductionDocumentNo { get; set; }
    [StringLength(30)]
    public string? ProductionOrderNo { get; set; }
    [Required, StringLength(30)]
    public string TransferPurpose { get; set; } = "MaterialSupply";
}

public sealed class ProductionTransferSuggestedLineDto
{
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string LineRole { get; set; } = string.Empty;
    public string ProductionOrderNo { get; set; } = string.Empty;
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
}

public sealed class ProductionTransferDetailDto
{
    public long Id { get; set; }
    public string DocumentNo { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    public string TransferPurpose { get; set; } = "MaterialSupply";
    public string? ProductionDocumentNo { get; set; }
    public string? ProductionOrderNo { get; set; }
    public string? SourceWarehouseCode { get; set; }
    public string? TargetWarehouseCode { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public bool CanDelete { get; set; }
    public string? DeleteBlockedReason { get; set; }
    public List<ProductionTransferDetailLineDto> Lines { get; set; } = new();
}

public sealed class ProductionTransferDetailLineDto
{
    public long Id { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public decimal Quantity { get; set; }
    public string LineRole { get; set; } = "ConsumptionSupply";
    public string? SourceCellCode { get; set; }
    public string? TargetCellCode { get; set; }
    public string? ProductionOrderNo { get; set; }
}

public sealed class CreatePtHeaderDto : BaseHeaderCreateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class UpdatePtHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(20)]
    public string? CustomerCode { get; set; }
    [StringLength(20)]
    public string? SourceWarehouse { get; set; }
    [StringLength(20)]
    public string? TargetWarehouse { get; set; }
}

public sealed class PtAssignedProductionTransferOrderLinesDto
{
    public IEnumerable<PtLineDto> Lines { get; set; } = Array.Empty<PtLineDto>();
    public IEnumerable<PtLineSerialDto> LineSerials { get; set; } = Array.Empty<PtLineSerialDto>();
    public IEnumerable<PtImportLineDto> ImportLines { get; set; } = Array.Empty<PtImportLineDto>();
    public IEnumerable<PtRouteDto> Routes { get; set; } = Array.Empty<PtRouteDto>();
}

public sealed class CreatePtLineWithKeyDto : BaseLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGuid { get; set; }
    public long? OrderId { get; set; }
    public string? ErpLineReference { get; set; }
}

public sealed class CreatePtLineSerialWithLineKeyDto : BaseLineSerialCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreatePtRouteWithLineKeyDto : BaseRouteCreateDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? ImportLineClientKey { get; set; }
    public Guid? ImportLineGroupGuid { get; set; }
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string StockCode { get; set; } = string.Empty;
    public string? YapKod { get; set; }
    public string? Description { get; set; }
}

public sealed class CreatePtImportLineWithKeysDto : BaseImportLineCreateDto
{
    public string? ClientKey { get; set; }
    public Guid? ClientGroupGuid { get; set; }
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
    public string? RouteClientKey { get; set; }
    public Guid? RouteGroupGuid { get; set; }
}

public sealed class CreatePtTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class GenerateProductionTransferOrderRequestDto
{
    [Required]
    public CreatePtHeaderDto Header { get; set; } = null!;
    public List<CreatePtLineWithKeyDto>? Lines { get; set; }
    public List<CreatePtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePtTerminalLineWithUserDto>? TerminalLines { get; set; }
}

public sealed class BulkPtGenerateRequestDto
{
    [Required]
    public CreatePtHeaderDto Header { get; set; } = null!;
    public List<CreatePtLineWithKeyDto>? Lines { get; set; }
    public List<CreatePtLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreatePtRouteWithLineKeyDto>? Routes { get; set; }
    public List<CreatePtImportLineWithKeysDto>? ImportLines { get; set; }
}
