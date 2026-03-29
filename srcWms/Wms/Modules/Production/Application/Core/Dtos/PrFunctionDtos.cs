namespace Wms.Application.Production.Dtos;

public sealed class ProductHeaderDto
{
    public string? IsEmriNo { get; set; }
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? YapKod { get; set; }
    public string? YapAcik { get; set; }
    public decimal? Quantity { get; set; }
    public int? Priority { get; set; }
    public string? RefIsEmriNo { get; set; }
}

public sealed class ProductLineDto
{
    public string? MamulKodu { get; set; }
    public string? MamulAdi { get; set; }
    public string? IsEmriNo { get; set; }
    public string? SiparisNo { get; set; }
    public string? HamKodu { get; set; }
    public string? HamMaddeAdi { get; set; }
    public decimal? BirimMiktar { get; set; }
    public decimal? HesaplananToplamMiktar { get; set; }
    public string? OpNo { get; set; }
}
