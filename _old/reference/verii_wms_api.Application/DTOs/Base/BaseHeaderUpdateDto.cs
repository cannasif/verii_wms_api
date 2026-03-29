using System;
using System.ComponentModel.DataAnnotations;

namespace WMS_WEBAPI.DTOs
{
    public class BaseHeaderUpdateDto
    {
        public string? BranchCode { get; set; }
        public string? ProjectCode { get; set; }
        public string? OrderId { get; set; }
        public string? DocumentType { get; set; }
        public string? YearCode { get; set; }
        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description1 { get; set; }

        // Açıklama alanı (maks. 100 karakter)
        [StringLength(100)]
        public string? Description2 { get; set; }
        public byte? PriorityLevel { get; set; }
        public DateTime? PlannedDate { get; set; }
        public bool? IsPlanned { get; set; }
    }
}
