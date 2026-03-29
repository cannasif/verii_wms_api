namespace Wms.Domain.Entities.Production.Functions;

public sealed class FnProductLine
{
    public string? MAMUL_KODU { get; set; }
    public string? MAMUL_ADI { get; set; }
    public string? ISEMRINO { get; set; }
    public string? SIPARIS_NO { get; set; }
    public string? HAM_KODU { get; set; }
    public string? HAM_MADDE_ADI { get; set; }
    public decimal? BIRIM_MIKTAR { get; set; }
    public decimal? HESAPLANAN_TOPLAM_MIKTAR { get; set; }
    public string? OPNO { get; set; }
}
