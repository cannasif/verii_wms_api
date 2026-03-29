using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.GoodsReceipt;

public sealed class GrImportDocument : BaseEntity
{
    public long HeaderId { get; set; }
    public GrHeader Header { get; set; } = null!;
    public byte[] Base64 { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public string? FileName { get; set; }
}
