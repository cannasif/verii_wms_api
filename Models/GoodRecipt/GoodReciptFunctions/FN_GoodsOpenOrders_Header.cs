using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿namespace WMS_WEBAPI.Models
{
    public class FN_GoodsOpenOrders_Header
    {
        public string Mode { get; set; } = string.Empty;   // varchar(1), not null

        public string SiparisNo { get; set; } = string.Empty;   // TDBBELGENO (varchar(15)), not null

        public int? OrderID { get; set; }   // int, nullable

        public string? CustomerCode { get; set; }   // varchar(30), nullable

        public string? CustomerName { get; set; }   // varchar(100), nullable

        public short? BranchCode { get; set; }   // smallint, nullable

        public short? TargetWh { get; set; }   // smallint, nullable

        public string? ProjectCode { get; set; }   // varchar(50), nullable

        public DateTime? OrderDate { get; set; }   // datetime, nullable

        public decimal? OrderedQty { get; set; }   // decimal(18,4), nullable

        public decimal? DeliveredQty { get; set; }   // decimal(18,4), nullable

        public decimal? RemainingHamax { get; set; }   // decimal(18,4), nullable

        public decimal PlannedQtyAllocated { get; set; }   // decimal(18,4), not null

        public decimal? RemainingForImport { get; set; }   // decimal(18,4), nullable
    }
}
