namespace WMS_WEBAPI.Models
{
    public class RII_FN_ONHANDQUANTITY
    {
        public int DEPO_KODU { get; set; }              // int, NOT NULL varsaydım
        public string? STOK_KODU { get; set; }          // varchar, NULL olabilir
        public string? PROJE_KODU { get; set; }         // varchar, NULL olabilir
        public string? SERI_NO { get; set; }            // varchar, NULL olabilir
        public string? HUCRE_KODU { get; set; }         // varchar, NULL olabilir
        public string? KAYNAK { get; set; }             // varchar, NULL olabilir
        public decimal? BAKIYE { get; set; }            // decimal, NULL olabilir
        public string? STOK_ADI { get; set; }           // varchar, NULL olabilir
        public string? DEPO_ISMI { get; set; }          // varchar, NULL olabilir
    }
}