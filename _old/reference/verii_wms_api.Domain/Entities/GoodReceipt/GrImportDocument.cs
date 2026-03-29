using System;

namespace WMS_WEBAPI.Models
{
    public class GrImportDocument : BaseEntity
    {
        public long HeaderId { get; set; }
        public virtual GrHeader Header { get; set; } = null!;

        public byte[] Base64 { get; set; } = null!;

        public string? ImageUrl { get; set; } = null;
        
        public string? FileName { get; set; } = null;

    }
}
