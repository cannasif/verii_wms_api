using System;

namespace WMS_WEBAPI.DTOs
{
    public class BaseHeaderEntityDto : BaseEntityDto
    {
        public string YearCode { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string? ProjectCode { get; set; }
        public string? OrderId { get; set; }
        public DateTime PlannedDate { get; set; }
        public bool IsPlanned { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public byte? PriorityLevel { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPendingApproval { get; set; }
        public bool? ApprovalStatus { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsERPIntegrated { get; set; }
        public string? ERPReferenceNumber { get; set; }
        public DateTime? ERPIntegrationDate { get; set; }
        public string? ERPIntegrationStatus { get; set; }
        public string? ERPErrorMessage { get; set; }
    }
}
