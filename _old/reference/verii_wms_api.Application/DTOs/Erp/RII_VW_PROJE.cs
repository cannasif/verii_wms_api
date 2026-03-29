using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace WMS_WEBAPI.Models
{
    public class RII_VW_PROJE
    {
        [Column("PROJE_KODU")]
        [MaxLength(15)]
        public string ProjeKod { get; set; } = null!;
        
        [Column("PROJE_ACIKLAMA")]
        [MaxLength(50)]
        public string? ProjeAciklama { get; set; }
    }
}