using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace WMS_WEBAPI.Models
{
    public class RII_FN_OPENGOODSFORORDERDETAILBYORDERNUMBERS
    {
        [MaxLength(35)]
        public string STOK_KODU { get; set; } = string.Empty;      // varchar(35), NOT NULL
        
        [Column(TypeName = "decimal(29,8)")]
        public decimal? KALAN_MIKTAR { get; set; }                 // decimal(29,8), NULL
        
        public short? DEPO_KODU { get; set; }                      // smallint, NULL
        
        [MaxLength(20)]
        public string? DEPO_ISMI { get; set; }                     // varchar(20), NULL
        
        [MaxLength(100)]
        public string? STOK_ADI { get; set; }                      // varchar(100), NULL
        
        [MaxLength(1)]
        public string GIRIS_SERI { get; set; } = string.Empty;     // char(1), NOT NULL
        
        [MaxLength(1)]
        public string SERI_MIK { get; set; } = string.Empty;       // char(1), NOT NULL
    }
}