using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Package.Dtos;

public sealed class PHeaderDto : BaseEntityDto
{
    public string? WarehouseCode { get; set; }
    public string PackingNo { get; set; } = null!;
    public DateTime? PackingDate { get; set; }
    public string? SourceType { get; set; }
    public long? SourceHeaderId { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerAddress { get; set; }
    public string Status { get; set; } = null!;
    public decimal? TotalPackageCount { get; set; }
    public decimal? TotalQuantity { get; set; }
    public decimal? TotalNetWeight { get; set; }
    public decimal? TotalGrossWeight { get; set; }
    public decimal? TotalVolume { get; set; }
    public long? CarrierId { get; set; }
    public string? CarrierServiceType { get; set; }
    public string? TrackingNo { get; set; }
    public bool IsMatched { get; set; }
}

public sealed class CreatePHeaderDto
{
    [MaxLength(20)] public string? WarehouseCode { get; set; }
    [Required, MaxLength(50)] public string PackingNo { get; set; } = null!;
    public DateTime? PackingDate { get; set; }
    [MaxLength(30)] public string? SourceType { get; set; }
    public long? SourceHeaderId { get; set; }
    [MaxLength(50)] public string? CustomerCode { get; set; }
    [MaxLength(255)] public string? CustomerAddress { get; set; }
    [MaxLength(20)] public string? Status { get; set; }
    public decimal? TotalPackageCount { get; set; }
    public decimal? TotalQuantity { get; set; }
    public decimal? TotalNetWeight { get; set; }
    public decimal? TotalGrossWeight { get; set; }
    public decimal? TotalVolume { get; set; }
    public long? CarrierId { get; set; }
    [MaxLength(20)] public string? CarrierServiceType { get; set; }
    [MaxLength(100)] public string? TrackingNo { get; set; }
    public bool? IsMatched { get; set; }
}

public sealed class UpdatePHeaderDto
{
    [MaxLength(20)] public string? WarehouseCode { get; set; }
    [MaxLength(50)] public string? PackingNo { get; set; }
    public DateTime? PackingDate { get; set; }
    [MaxLength(30)] public string? SourceType { get; set; }
    public long? SourceHeaderId { get; set; }
    [MaxLength(50)] public string? CustomerCode { get; set; }
    [MaxLength(255)] public string? CustomerAddress { get; set; }
    [MaxLength(20)] public string? Status { get; set; }
    public decimal? TotalPackageCount { get; set; }
    public decimal? TotalQuantity { get; set; }
    public decimal? TotalNetWeight { get; set; }
    public decimal? TotalGrossWeight { get; set; }
    public decimal? TotalVolume { get; set; }
    public long? CarrierId { get; set; }
    [MaxLength(20)] public string? CarrierServiceType { get; set; }
    [MaxLength(100)] public string? TrackingNo { get; set; }
    public bool? IsMatched { get; set; }
}

public sealed class MatchPlinesRequestDto
{
    [Required]
    public bool IsMatched { get; set; }
}

public sealed class PPackageDto : BaseEntityDto
{
    public long PackingHeaderId { get; set; }
    public string PackageNo { get; set; } = null!;
    public string PackageType { get; set; } = null!;
    public string? Barcode { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Volume { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? TareWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public bool IsMixed { get; set; }
    public string Status { get; set; } = null!;
}

public sealed class CreatePPackageDto
{
    [Required] public long PackingHeaderId { get; set; }
    [Required, MaxLength(50)] public string PackageNo { get; set; } = null!;
    [MaxLength(20)] public string? PackageType { get; set; }
    [MaxLength(100)] public string? Barcode { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Volume { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? TareWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public bool? IsMixed { get; set; }
    [MaxLength(20)] public string? Status { get; set; }
}

public sealed class UpdatePPackageDto
{
    [MaxLength(50)] public string? PackageNo { get; set; }
    [MaxLength(20)] public string? PackageType { get; set; }
    [MaxLength(100)] public string? Barcode { get; set; }
    public decimal? Length { get; set; }
    public decimal? Width { get; set; }
    public decimal? Height { get; set; }
    public decimal? Volume { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? TareWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public bool? IsMixed { get; set; }
    [MaxLength(20)] public string? Status { get; set; }
}

public sealed class PLineDto : BaseEntityDto
{
    public long PackingHeaderId { get; set; }
    public long PackageId { get; set; }
    public string? Barcode { get; set; }
    public string StockCode { get; set; } = null!;
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public decimal Quantity { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public long? SourceRouteId { get; set; }
}

public sealed class CreatePLineDto
{
    [Required] public long PackingHeaderId { get; set; }
    [Required] public long PackageId { get; set; }
    [MaxLength(50)] public string? Barcode { get; set; }
    [Required, MaxLength(50)] public string StockCode { get; set; } = null!;
    [MaxLength(50)] public string? YapKod { get; set; }
    [Required] public decimal Quantity { get; set; }
    [MaxLength(50)] public string? SerialNo { get; set; }
    [MaxLength(50)] public string? SerialNo2 { get; set; }
    [MaxLength(50)] public string? SerialNo3 { get; set; }
    [MaxLength(50)] public string? SerialNo4 { get; set; }
    public long? SourceRouteId { get; set; }
}

public sealed class UpdatePLineDto
{
    [MaxLength(50)] public string? Barcode { get; set; }
    [MaxLength(50)] public string? StockCode { get; set; }
    [MaxLength(50)] public string? YapKod { get; set; }
    public decimal? Quantity { get; set; }
    [MaxLength(50)] public string? SerialNo { get; set; }
    [MaxLength(50)] public string? SerialNo2 { get; set; }
    [MaxLength(50)] public string? SerialNo3 { get; set; }
    [MaxLength(50)] public string? SerialNo4 { get; set; }
    public long? SourceRouteId { get; set; }
}
