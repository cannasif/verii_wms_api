namespace Wms.Domain.Entities.ProductionTransfer.Functions;

public sealed class FnProductHeader
{
    public string? ISEMRINO { get; set; }
    public string? STOK_KODU { get; set; }
    public string? STOK_ADI { get; set; }
    public string? YAPKOD { get; set; }
    public string? YAPACIK { get; set; }
    public decimal? MIKTAR { get; set; }
    public int? ONCELIK { get; set; }
    public string? REFISEMRINO { get; set; }
}
