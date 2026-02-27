using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    [Table("RII_GR_IMPORT_DOCUMENT")]
    public class GrImportDocument : BaseEntity
    {
        public long HeaderId { get; set; }
        [ForeignKey(nameof(HeaderId))]
        public virtual GrHeader Header { get; set; } = null!;

        [Required]
        public byte[] Base64 { get; set; } = null!;

        public string? ImageUrl { get; set; } = null;
        
        public string? FileName { get; set; } = null;

    }
}
