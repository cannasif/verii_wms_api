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

        /// <summary>
        /// Belgeyi tamamlanmış olarak işaretler.
        /// </summary>
        public void MarkAsCompleted()
        {
            IsCompleted = true;
            CompletionDate = DateTimeProvider.Now;
        }

        /// <summary>
        /// Tamamlanma bilgisini geri alır (belgeyi tekrar beklemede yapar).
        /// </summary>
        public void UndoCompletion()
        {
            IsCompleted = false;
            CompletionDate = null;
        }

        /// <summary>
        /// Onay bekleme durumunu ayarlar.
        /// </summary>
        /// <param name="pending">Onay bekliyor mu?</param>
        public void SetPendingApproval(bool pending)
        {
            IsPendingApproval = pending;
        }

        /// <summary>
        /// Belgeyi onaylanmış olarak işaretler.
        /// </summary>
        /// <param name="approvedByUserId">Onaylayan kullanıcı ID'si.</param>
        public void Approve(long approvedByUserId)
        {
            ApprovalStatus = true;
            ApprovedByUserId = approvedByUserId;
            ApprovalDate = DateTimeProvider.Now;
        }

        /// <summary>
        /// Belgeyi reddedilmiş olarak işaretler.
        /// </summary>
        /// <param name="rejectedByUserId">Reddeden kullanıcı ID'si.</param>
        public void Reject(long rejectedByUserId)
        {
            ApprovalStatus = false;
            ApprovedByUserId = rejectedByUserId;
            ApprovalDate = DateTimeProvider.Now;
        }

        /// <summary>
        /// ERP entegrasyonunun başarılı olduğunu kaydeder.
        /// </summary>
        /// <param name="referenceNumber">ERP referans numarası.</param>
        public void MarkErpIntegrated(string referenceNumber)
        {
            IsERPIntegrated = true;
            ERPReferenceNumber = referenceNumber;
            ERPIntegrationDate = DateTimeProvider.Now;
            ERPIntegrationStatus = "Success";
        }

        /// <summary>
        /// ERP entegrasyonunun başarısız olduğunu ve hata bilgisini kaydeder.
        /// </summary>
        /// <param name="errorMessage">ERP hata mesajı.</param>
        public void SetErpError(string errorMessage)
        {
            ERPIntegrationStatus = "Failed";
            ERPErrorMessage = errorMessage;
        }
    }
}
