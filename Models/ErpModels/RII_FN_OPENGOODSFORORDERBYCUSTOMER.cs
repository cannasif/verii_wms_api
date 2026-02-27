using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace WMS_WEBAPI.Models
{
    public class RII_FN_OPENGOODSFORORDERBYCUSTOMER
    {
        [MaxLength(15)]
        public string FATIRS_NO { get; set; } = string.Empty;          // varchar(15), NOT NULL
        
        public DateTime TARIH { get; set; }                            // datetime, NOT NULL
        
        [Column(TypeName = "decimal(28,8)")]
        public decimal? BRUTTUTAR { get; set; }                        // decimal(28,8), NULL
    }
}