using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace WMS_WEBAPI.Models
{
    public class RII_VW_DEPO
    {
        public short DEPO_KODU { get; set; }
        public string? DEPO_ISMI { get; set; }
        public char? DEPO_KILITLE { get; set; }
        public string? CARI_KODU { get; set; }
        public char? EKSIBAKIYE { get; set; }
        public char? FIAT_TIPI { get; set; }
        public short SUBE_KODU { get; set; }
        public string? S_YEDEK1 { get; set; }
        public string? S_YEDEK2 { get; set; }
        public short? I_YEDEK1 { get; set; }
        public short? I_YEDEK2 { get; set; }
        public char? C_YEDEK1 { get; set; }
        public char? C_YEDEK2 { get; set; }
        public DateTime? D_YEDEK1 { get; set; }
        public string? KAYITYAPANKUL { get; set; }
        public DateTime? KAYITTARIHI { get; set; }
        public string? DUZELTMEYAPANKUL { get; set; }
        public DateTime? DUZELTMETARIHI { get; set; }
        public char? EMANETDEPO { get; set; }
        public short KILIT_POLITIKASI { get; set; }
    }
}