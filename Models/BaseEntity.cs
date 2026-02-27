using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    
        // User Informations
        public long? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }

        public long? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }

        public long? DeletedBy { get; set; }
        [ForeignKey("DeletedBy")]
        public User? DeletedByUser { get; set; }

    }
}
