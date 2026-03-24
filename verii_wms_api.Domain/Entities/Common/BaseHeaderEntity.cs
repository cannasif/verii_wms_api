using System;

namespace WMS_WEBAPI.Models
{
    public abstract class BaseHeaderEntity : BaseEntity
    {

        public string? DocumentNo { get; set; }

        public DateTime? DocumentDate { get; set; }

        // Yıl kodu – ERP’de dönemsel kayıtlar için kullanılır (örn. 2025)
        public string YearCode { get; set; } = DateTime.Now.Year.ToString();

        // Şube kodu – ERP’deki şube veya işyeri kodunu temsil eder (örnek: "01")
        public string BranchCode { get; set; } = "0";

        public string? OrderId { get; set; }

        // Proje kodu – Eğer üretim/transfer belirli bir projeye aitse kullanılır
        public string? ProjectCode { get; set; }
        
        // Planlanmış tarih – belge ne zaman planlanmışsa (örn. 2025-01-01)
        public DateTime? PlannedDate { get; set; }
        public bool IsPlanned { get; set; } = false; // Planlı giriş mi?

        // Belge tipi – örneğin:
        public string DocumentType { get; set; } = null!;


        // Sayısal öncelik değeri (örneğin 1 = yüksek, 3 = düşük)
        public byte? PriorityLevel { get; set; }

              
        // Completion Date (specific)
        public DateTime? CompletionDate { get; set; } // kayıt tamamlanma tarihi
        public bool IsCompleted { get; set; } = false;
        

        // Approval Fields (ERP specific)
        public bool IsPendingApproval { get; set; } = false; // Onaya gönderilecek mi? default false
        public bool? ApprovalStatus { get; set; } // Onay durumu (true = Approved, false = Rejected, null = Pending)
        public long? ApprovedByUserId { get; set; } // Onaylayan kullanıcı ID
        public DateTime? ApprovalDate { get; set; } // Onay tarihi
        public bool IsERPIntegrated { get; set; } = false;


        // ERP Integration Fields
        public string? ERPReferenceNumber { get; set; } // ERP referans numarası
        public DateTime? ERPIntegrationDate { get; set; } // ERP entegrasyon tarihi
        public string? ERPIntegrationStatus { get; set; } // ERP entegrasyon durumu
        public string? ERPErrorMessage { get; set; } // ERP hata mesajı (varsa)


        // Açıklama alanları – kullanıcıya serbest bilgi notu veya ERP açıklamaları
        public string? Description1 { get; set; }

        public string? Description2 { get; set; }
    }
}
