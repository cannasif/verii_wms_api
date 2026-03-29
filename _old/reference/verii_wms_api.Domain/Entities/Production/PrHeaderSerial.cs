using System;

namespace WMS_WEBAPI.Models
{
    public class PrHeaderSerial : BaseEntity
    {
        public long HeaderId { get; set; }
        public virtual PrHeader Header { get; set; } = null!;

        public string? SerialNo { get; set; }

        public string? SerialNo2 { get; set; }

        public string? SerialNo3 { get; set; }

        public string? SerialNo4 { get; set; }

        public decimal? Amount { get; set; }
    }
}
