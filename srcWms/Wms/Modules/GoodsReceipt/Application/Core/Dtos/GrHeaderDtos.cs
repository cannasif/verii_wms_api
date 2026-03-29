using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.GoodsReceipt.Dtos;

public sealed class GrHeaderDto : BaseHeaderEntityDto
{
    [Required]
    [StringLength(30)]
    public string CustomerCode { get; set; } = null!;
    public string? CustomerName { get; set; }
    public bool ReturnCode { get; set; }
    public bool OCRSource { get; set; }
    [StringLength(100)]
    public string? Description3 { get; set; }
    [StringLength(100)]
    public string? Description4 { get; set; }
    [StringLength(100)]
    public string? Description5 { get; set; }
}

public sealed class CreateGrHeaderDto : BaseHeaderCreateDto
{
    [Required]
    [StringLength(30)]
    public string CustomerCode { get; set; } = null!;
    public bool ReturnCode { get; set; }
    public bool OCRSource { get; set; }
    [StringLength(100)]
    public string? Description3 { get; set; }
    [StringLength(100)]
    public string? Description4 { get; set; }
    [StringLength(100)]
    public string? Description5 { get; set; }
}

public sealed class UpdateGrHeaderDto : BaseHeaderUpdateDto
{
    [StringLength(30)]
    public string? CustomerCode { get; set; }
    public bool? ReturnCode { get; set; }
    public bool? OCRSource { get; set; }
    [StringLength(100)]
    public string? Description3 { get; set; }
    [StringLength(100)]
    public string? Description4 { get; set; }
    [StringLength(100)]
    public string? Description5 { get; set; }
}

public sealed class GrLineDto : BaseLineEntityDto
{
    public long HeaderId { get; set; }
    public long? OrderId { get; set; }
}

public sealed class CreateGrLineWithKeyDto : BaseLineCreateDto
{
    [Required]
    public string ClientKey { get; set; } = null!;
}

public sealed class GrImportLineDto : BaseImportLineEntityDto
{
    public long? LineId { get; set; }
    public long HeaderId { get; set; }
}

public sealed class CreateGrImportLineWithLineKeyDto : BaseImportLineCreateDto
{
    public string? LineClientKey { get; set; }
    [Required]
    public string ClientKey { get; set; } = null!;
    public string? StockName { get; set; }
    public string YapAcik { get; set; } = string.Empty;
    public string? Unit { get; set; }
}

public sealed class GrRouteDto : BaseRouteEntityDto
{
    public long ImportLineId { get; set; }
    public string? Description { get; set; }
    public long? PackageLineId { get; set; }
    public string? PackageNo { get; set; }
    public long? PackageHeaderId { get; set; }
}

public sealed class CreateGrRouteWithImportLineKeyDto : BaseRouteCreateDto
{
    [Required]
    public string ImportLineClientKey { get; set; } = null!;
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    [StringLength(100)]
    public string? Description { get; set; }
}

public sealed class GrLineSerialDto : BaseLineSerialEntityDto
{
    public long? LineId { get; set; }
    public string? ClientKey { get; set; }
}

public class CreateGrLineSerialDto : BaseLineSerialCreateDto
{
    public long? LineId { get; set; }
    public string? ClientKey { get; set; }
}

public sealed class CreateGrImportSerialLineWithImportLineKeyDto : BaseLineSerialCreateDto
{
    [Required]
    public string ImportLineClientKey { get; set; } = null!;
}

public sealed class CreateGrLineSerialWithLineKeyDto : CreateGrLineSerialDto
{
    public string? LineClientKey { get; set; }
    public Guid? LineGroupGuid { get; set; }
}

public sealed class CreateGrTerminalLineWithUserDto : BaseTerminalLineCreateDto
{
}

public sealed class CreateGrImportDocumentSimpleDto
{
    [Required]
    public byte[] Base64 { get; set; } = null!;
}

public sealed class GrAssignedOrderLinesDto
{
    public IEnumerable<GrLineDto> Lines { get; set; } = Array.Empty<GrLineDto>();
    public IEnumerable<GrLineSerialDto> LineSerials { get; set; } = Array.Empty<GrLineSerialDto>();
    public IEnumerable<GrImportLineDto> ImportLines { get; set; } = Array.Empty<GrImportLineDto>();
    public IEnumerable<GrRouteDto> Routes { get; set; } = Array.Empty<GrRouteDto>();
}

public sealed class BulkCreateGrRequestDto
{
    [Required]
    public CreateGrHeaderDto Header { get; set; } = null!;
    public List<CreateGrImportDocumentSimpleDto>? Documents { get; set; }
    public List<CreateGrLineWithKeyDto>? Lines { get; set; }
    public List<CreateGrImportLineWithLineKeyDto>? ImportLines { get; set; }
    public List<CreateGrImportSerialLineWithImportLineKeyDto>? SerialLines { get; set; }
    public List<CreateGrRouteWithImportLineKeyDto>? Routes { get; set; }
}

public sealed class GenerateGoodReceiptOrderRequestDto
{
    [Required]
    public CreateGrHeaderDto Header { get; set; } = null!;
    public List<CreateGrLineWithKeyDto>? Lines { get; set; }
    public List<CreateGrLineSerialWithLineKeyDto>? LineSerials { get; set; }
    public List<CreateGrTerminalLineWithUserDto>? TerminalLines { get; set; }
}
